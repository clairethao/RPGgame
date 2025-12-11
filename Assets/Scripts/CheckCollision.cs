using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    int damage = 20;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        damage = GameObject.Find("gameManager").GetComponent<GameManager>().player.power / 2;
        if (other.GetComponent<Collider>().gameObject.tag == "target")
        {
            other.GetComponent<Collider>().gameObject.GetComponent<ManageTargetHealth>().DecreaseHealth(damage);
        }
    }
}
