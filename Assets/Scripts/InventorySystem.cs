using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    List<Item> playerInventory;
    int currentInventoryIndex = 0;
    bool isVisible = false;
    GameObject inventoryText, inventoryImage, inventoryDescription, inventoryPanel;

    void Start()
    {
        inventoryText = GameObject.Find("inventoryText");
        inventoryImage = GameObject.Find("inventoryImage");
        inventoryDescription = GameObject.Find("inventoryDescription");
        inventoryPanel = GameObject.Find("inventoryPanel");

        if (inventoryText == null) Debug.LogWarning("inventoryText not found in this scene.");
        if (inventoryImage == null) Debug.LogWarning("inventoryImage not found in this scene.");
        if (inventoryDescription == null) Debug.LogWarning("inventoryDescription not found in this scene.");
        if (inventoryPanel == null) Debug.LogWarning("inventoryPanel not found in this scene.");

        DisplayUI(false);

        playerInventory = new List<Item>();
        playerInventory.Add(new Item(Item.ItemType.MEAT));
        playerInventory.Add(new Item(Item.ItemType.GOLD));

        playerInventory[1].nb = 300;
        checkInventory();
    }

    void Update()
    {
        if (isVisible)
        {
            DisplayUI(true);
            Item currentItem = playerInventory[currentInventoryIndex];

            GameObject.Find("inventoryText").GetComponent<Text>().text = currentItem.name + "[" + currentItem.nb + "]";
            GameObject.Find("inventoryDescription").GetComponent<Text>().text = currentItem.description + "\n\n Press [U] to Use/Select";
            GameObject.Find("inventoryImage").GetComponent<RawImage>().texture = currentItem.GetTexture();
            if (Input.GetKeyDown(KeyCode.I)) currentInventoryIndex++;
            if (currentInventoryIndex >= playerInventory.Count)
            {
                currentInventoryIndex = 0;
                isVisible = false;
                DisplayUI(false);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (playerInventory[currentInventoryIndex].familyType == Item.ItemFamilyType.FOOD)
                {
                    GetComponent<ControlPlayer>().IncreaseHealth(playerInventory[currentInventoryIndex].healthBenefits);
                    playerInventory.RemoveAt(currentInventoryIndex);
                    currentInventoryIndex = 0; isVisible = false; DisplayUI(false);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I)) isVisible = true;
        }

    }

    void checkInventory()
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            print(playerInventory[i].ItemInfo());

        }
    }

    void DisplayUI(bool toggle)
    {
        if (inventoryText != null) inventoryText.SetActive(toggle);
        if (inventoryPanel != null) inventoryPanel.SetActive(toggle);
        if (inventoryImage != null) inventoryImage.SetActive(toggle);
        if (inventoryDescription != null) inventoryDescription.SetActive(toggle);
    }

    public bool UpdateItem(Item.ItemType type, int nbItemsToAdd)
    {
        bool foundSimilarItem = false;

        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].type == type)
            {
                if (playerInventory[i].nb + nbItemsToAdd <= playerInventory[i].maxNb)
                {
                    playerInventory[i].nb += nbItemsToAdd; foundSimilarItem = true;
                    break;
                }
                else return false;
            }
        }
        if (!foundSimilarItem) { playerInventory.Add(new Item(type)); playerInventory[playerInventory.Count - 1].nb = nbItemsToAdd; }
        return true;
    }

    public int GetMoney()
    {
        for (int i=0; i < playerInventory.Count; ++i)
        {
            if (playerInventory[i].type == Item.ItemType.GOLD)
            {
                return (playerInventory[i].nb);
            }
        }
        return 0;
    }

    public void AddPurchasedItems(List<Item> purchasedItems)
    {
        bool t;
        for (int i=0;i < purchasedItems.Count; ++i)
        {
            if (purchasedItems[i].nb > 0) t = UpdateItem(purchasedItems[i].type, purchasedItems[i].nb);
        }
    }

    public void SetMoney(int newAmount)
    {
        for (int i=0; i < playerInventory.Count; ++i)
        {
            if (playerInventory[i].type == Item.ItemType.GOLD)
            {
                playerInventory[i].nb = newAmount;
            }
        }
    }
}
