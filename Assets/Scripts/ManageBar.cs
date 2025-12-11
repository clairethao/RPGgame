using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageBar : MonoBehaviour
{
    int value = 0;
    string label;
    
    void Start()
    {
        label = "Health";
        UpdateValue();
        transform.Find("label").GetComponent<Text>().text = label;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B)) IncreaseValue(10);
    }

    public void IncreaseValue(int amount)
    {
        value += amount;
        if (value > 100) value = 100;
        UpdateValue();
    }

    void UpdateValue()
    {
        transform.Find("fill").transform.localScale = new Vector3(value / 100.0f, transform.localScale.y, transform.localScale.z);
        transform.Find("text").GetComponent<Text>().text = "" + value;
    }

    public void SetValue(int amount)
    {
        value = amount;
        UpdateValue();
    }
}
