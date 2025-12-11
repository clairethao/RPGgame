using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTargetHealth : MonoBehaviour
{
    [Range(100,1000)]
    public int health =100;
    float hiTimer;
    bool hitFlash;
    float alpha;

    void Start()
    {
        alpha = 0.0f;
        gameObject.transform.Find("Sphere").GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, alpha);
    }

    void Update()
    {
        if (hitFlash)
        {
            alpha -= Time.deltaTime;
            gameObject.transform.Find("Sphere").GetComponent<Renderer>().material.color = new Color(1, 0, 0, alpha);
            if (alpha <= 0)
            {
                hitFlash = false;
                alpha = 0;
            }
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
        if (this.health <= 0)
        {
            this.health = 0;
            DestroyTarget();
        }
    }

    public int GetHealth()
    {
        return (this.health);
    }

    void DestroyTarget()
    {
        GetComponent<ControlNPCGaurd>().Dies();
        Destroy(gameObject, 5);
        GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.destroy_one, gameObject.name);
    }

    public void DecreaseHealth(int increment)
    {
        SetHealth(this.health - increment);
        hitFlash = true;
        alpha = 0.5f;
    }
}
