using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script unlocks a level button and shows the number of stars achieved

public class CartoonLevelButton : MonoBehaviour
{
    public bool isLocked;

    public int starNumber; 
    public Sprite[] stars;

    private Image starsIcon;

    private Button button;
    public GameObject number;
    public GameObject lockIcon;



    void Awake()
    {

        starsIcon = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();

        button = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
    }

    void OnEnable()
    {
        if (isLocked)
        {

            button.interactable = false;
            number.SetActive(false);
            lockIcon.SetActive(true);

        }
        else {

            button.interactable = true;
            number.SetActive(true);
            lockIcon.SetActive(false);

        }

    }

    void Update()
    {
        if (!isLocked) {
            //change 3stars icon
            if (starNumber >= 0 && starNumber < stars.Length)
            {

                starsIcon.sprite = stars[starNumber];
            }
        }
    }


}
