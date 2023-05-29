using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script makes an item abailable on the shop

public class CartoonShop : MonoBehaviour
{
    public bool isAvailable;

    public Sprite iconOff;
    public Sprite iconOn;

    public Button button;
    public Image icon;



    //-------

    void OnEnable()
    {
        if (!isAvailable)
        {
            icon.sprite = iconOff;
            button.interactable = false;

        }
        else
        {

            icon.sprite = iconOn;
            button.interactable = true;
        }
    }
}
