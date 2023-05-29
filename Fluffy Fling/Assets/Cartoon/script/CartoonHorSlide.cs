using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script slides between groups of buttons

public class CartoonHorSlide : MonoBehaviour
{
    public AnimationCurve curve;

    public float animationTime = 0.25f;

    //private RectTransform scroller;
    private RectTransform element;

    private RectTransform content;
    private HorizontalLayoutGroup horGroup;

    private float startPos;
    private float targetPos;

    //-------

    void Awake()
    {
        content = GetComponent<RectTransform>();
        horGroup = GetComponent<HorizontalLayoutGroup>();
        element = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }


    void OnEnable()
    {
        //calculate content size
        int chNumb = gameObject.transform.childCount;
        content.sizeDelta = new Vector2(element.sizeDelta.x * chNumb + horGroup.spacing * (chNumb - 1) , content.sizeDelta.y);
    }

    //-------

    //sliding to the closest position
    IEnumerator EndDragClamp()
    {

        float eTime = 0f;

        float track = targetPos - startPos;

        while (eTime < animationTime)
        {

            float xPos = curve.Evaluate(eTime / animationTime) * track;

            content.anchoredPosition = new Vector2(startPos + xPos, content.anchoredPosition.y);
            
            yield return null;

            eTime += Time.deltaTime;

        }

        content.anchoredPosition = new Vector2(targetPos, content.anchoredPosition.y);

    }

    //calculate position
    public void OnDragEndClamp()
    {

        startPos = content.anchoredPosition.x;

        float cSize = horGroup.spacing + element.sizeDelta.x;

        targetPos = Mathf.Round(startPos/cSize) * cSize;


        StartCoroutine("EndDragClamp");
    }

}
