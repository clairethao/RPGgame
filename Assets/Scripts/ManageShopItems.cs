using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageShopItems : MonoBehaviour
{
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void IncreaseQuantity()
    {
        print("Just Clicked");
        transform.parent.GetComponent<ShopItem>().IncreaseQuantity();
    }

    public void DecreaseQuantity()
    {
        transform.parent.GetComponent<ShopItem>().DecreaseQuantity();
    }
}
