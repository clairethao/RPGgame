using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationToReach : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject.Find("gameManager").GetComponent<QuestSystem>().Notify(QuestSystem.possibleActions.enter_place_called, gameObject.name);
        }
    }
}
