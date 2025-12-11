using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlPlayer : MonoBehaviour
{
    float speed, rotationAroundY;
    Animator anim;
    CharacterController controller;
    AnimatorStateInfo info;
    bool isTalking = false;
    GameObject objectToPickup;
    bool itemToPickupNearBy = false;
    GameObject userMessage;
    GameObject healthUI, skillsUI, shopUI;
    [Header("Health Settings")]
    [Tooltip("Health value between 0 and 100.")]
    public int health = 50;
    public bool shopIsDisplayed;
    GameObject weapon;
    bool weaponIsActive = false;

    public void IncreaseHealth(int amount)
    {

        health += amount;
        if (health > 100) health = 100;
        print("Health: " + health);
        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        userMessage = GameObject.Find("userMessage");
        userMessage.SetActive(false);

        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
        shopUI = GameObject.Find("shopUI");
        shopUI.SetActive(false);
        weapon = GameObject.Find("playerWeapon").gameObject; weapon.SetActive(false);
    }

    private void Update()
    {
        if (!shopIsDisplayed)
        {
            if (isTalking) return;
            info = anim.GetCurrentAnimatorStateInfo(0);
            speed = Input.GetAxis("Vertical");
            rotationAroundY = Input.GetAxis("Horizontal");
            anim.SetFloat("speed", speed);
            gameObject.transform.Rotate(0, rotationAroundY, 0);
            if (speed > 0) controller.Move(transform.forward * speed * 2.0f * Time.deltaTime);
        
            if (itemToPickupNearBy)
            {
                if (Input.GetKeyDown(KeyCode.Y)) PickUpObject1();
                if (Input.GetKeyDown(KeyCode.N)) 
                {
                    GameObject.Find("userMessageText").GetComponent<Text>().text = "";
                    userMessage.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            weaponIsActive = !weaponIsActive;
            if (weaponIsActive) anim.SetTrigger("useWeapon");
            else anim.SetTrigger("putWeaponBack");
        }
        if (info.IsName("UseWeapon"))
        {
            if(info.normalizedTime % 1.0 >= .50f)
            {
                weapon.SetActive(true);
            }
        }

        if (info.IsName("PuttingWeaponBack"))
        {
            if (info.normalizedTime % 1.0 >= .80f)
            {
                weapon.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (weaponIsActive) anim.SetTrigger("attackWithWeapon");
        }
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.name == "Diana" && !isTalking)
        {
            hit.collider.gameObject.GetComponent<DialogueSystem>().startDialogue();
            isTalking = true;
            anim.SetFloat("speed", 0);
            hit.collider.isTrigger = true;
            hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 1, 2);
        }
    }

    public void EndTalking()
    {
        isTalking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "itemToBeCollected")
        {
            objectToPickup = other.gameObject;
            itemToPickupNearBy = true;
            PickUpObject2();
        }
        if (other.CompareTag("Shop"))
        {
            shopIsDisplayed = true;
            anim.SetFloat("speed", 0);
            displayShopUI();
            GameObject.Find("shopSystem").GetComponent<ShopSystem>().Init();
        }
    }

    void PickUpObject2()
    {
        string article = objectToPickup.GetComponent<ObjectToBeCollected>().item.article;
        string message = "You just found " + article + " " + objectToPickup.GetComponent<ObjectToBeCollected>().item.name + "\n Collect? (y/n)";
        userMessage.SetActive(true);
        GameObject.Find("userMessageText").GetComponent<Text>().text = message;
    }

    void PickUpObject1()
    {
        if (GetComponent<InventorySystem>().UpdateItem(objectToPickup.GetComponent<ObjectToBeCollected>().type, 1))
        {
            Destroy(objectToPickup);
            itemToPickupNearBy = false;

            GameObject.Find("userMessageText").GetComponent<Text>().text = "";
            userMessage.SetActive(false);
            
            GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.acquire_a, objectToPickup.GetComponent<ObjectToBeCollected>().item.name);
        }
        else
        {
            string message = "You can't pickup this item as you have reached your limit.";
            GameObject.Find("userMessageText").GetComponent<Text>().text = message;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        itemToPickupNearBy = false;

        if (userMessage.activeSelf)
        {
            GameObject.Find("userMessageText").GetComponent<Text>().text = "";
            userMessage.SetActive(false);
        }
    }

    public void displayShopUI()
    {
        shopUI.SetActive(true);
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 50;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
    }
}
