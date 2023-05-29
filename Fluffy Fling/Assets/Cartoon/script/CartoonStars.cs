using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script shows the number of stars gained after a level

public class CartoonStars : MonoBehaviour
{
    public int starNumber;

    public GameObject starOne;
    public GameObject starTwo;
    public GameObject starThree;

    private Image shine;


    private Color whiteColor = Color.white;


    //--------

    void Awake()
    {
        shine = GetComponent<Image>();
    }


    void OnEnable()
    {
        //reset
        shine.enabled = false;
        starOne.SetActive(false);
        starTwo.SetActive(false);
        starThree.SetActive(false);


        if (starNumber > 0 && starNumber <= 3)
        {

            StartCoroutine("StarAnimation");
        }
    }



    void Update()
    {
        if (shine.enabled) {

            whiteColor.a = 0.8f + Mathf.PingPong(Time.time/2.5f, 0.2f);

            shine.color = whiteColor;
        }


    }



    //-------

    IEnumerator StarAnimation() {

        yield return new WaitForSeconds(0.5f);

        starOne.SetActive(true);


        if (starNumber <= 1)
        {

            yield break;
        }
        else {

            yield return new WaitForSeconds(0.5f);
        }

        starTwo.SetActive(true);

        if (starNumber <= 2)
        {

            yield break;
        }
        else
        {

            yield return new WaitForSeconds(0.5f);
        }

        starThree.SetActive(true);
        shine.enabled = true;
    }

}
