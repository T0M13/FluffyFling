using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script unlocks the achievement 

public class CartoonAchievement : MonoBehaviour
{
    public bool isAchieved;
    public bool isRedeemed;

    public Sprite iconOff;
    public Sprite iconOn;

    public Button button;
    public Image icon;



    //-------

    void OnEnable()
    {
        if (!isAchieved)
        {
            icon.sprite = iconOff;
            button.interactable = false;

        }
        else {

            icon.sprite = iconOn;
            button.interactable = true;

            //already redeemed?
            if (isRedeemed)
            {

                button.gameObject.SetActive(false);
            }
        }



    }

}
