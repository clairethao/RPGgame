using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    string itemName;
    int price, quantity;
    public int index;

    void Start()
    {
        quantity = 0;
        UpdateQuantityLabel();
    }

    void Update()
    {
        
    }

    public void IncreaseQuantity()
    {
        if (!canClick()) return;
        quantity++;
        UpdateQuantityLabel();
    }

    public void DecreaseQuantity()
    {
        quantity--;
        if (quantity < 0) quantity = 0;
        UpdateQuantityLabel();
    }

    void UpdateQuantityLabel()
    {
        transform.Find("itemQty").GetComponent<Text>().text = "" + quantity;
        GameObject.Find("shopSystem").GetComponent<ShopSystem>().UpdateTotal(index, quantity);
    }

    bool canClick()
    {
        //return true;
        GameObject shopSystem = GameObject.Find("shopSystem");
        return shopSystem.GetComponent<ShopSystem>().canAddItemsTocart(this.index);
    }
}
