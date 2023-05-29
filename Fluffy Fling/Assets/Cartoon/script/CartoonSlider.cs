using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Thi script slides object to left or right using buttons

public class CartoonSlider : MonoBehaviour
{
    public AnimationCurve curve;

    public float animationTime = 0.25f;


    private RectTransform content;
    private RectTransform group;
    private HorizontalLayoutGroup horGroup;

    public Button leftArrow;
    public Button rightArrow;

    private float targetPosition;

    private float xWidth;
    private float xMax;

    //-------



    void Awake()
    {
        content = GetComponent<RectTransform>();
        horGroup = GetComponent<HorizontalLayoutGroup>();

        group = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        targetPosition = content.anchoredPosition.x;
        int chNumb = gameObject.transform.childCount - 1;

        xWidth = group.sizeDelta.x + horGroup.spacing;
        xMax = ((group.sizeDelta.x * chNumb) + (horGroup.spacing * chNumb)) * -1;

    }
    
    void Update()
    {
        CheckArrowButton();

    }
    
    //-------

    void CheckArrowButton()
    {

        leftArrow.interactable = true;
        rightArrow.interactable = true;

        if (Mathf.Approximately(content.anchoredPosition.x, 0f))
        {

            leftArrow.interactable = false;
        }

        if (Mathf.Approximately(content.anchoredPosition.x, xMax))
        {

            rightArrow.interactable = false;
        }

    }

    //-------

    //Right Arrow Button
    public void RightButtonSlide() {

        float pos = content.anchoredPosition.x % xWidth;

        if (targetPosition != content.anchoredPosition.x && Mathf.Approximately(pos, 0f))
        {
            targetPosition = content.anchoredPosition.x;
        }

        targetPosition -= xWidth;

        if (Mathf.Approximately(targetPosition, xMax))
        {

            rightArrow.interactable = false;
        }

        StartCoroutine("Slide");
    }

    //Left Arrow Button
    public void LeftButtonSlide()
    {
        float pos = content.anchoredPosition.x % xWidth;

        if (targetPosition != content.anchoredPosition.x && Mathf.Approximately(pos, 0f))
        {
            targetPosition = content.anchoredPosition.x;
        }

        targetPosition += xWidth;

        if (Mathf.Approximately(targetPosition, 0f))
        {

            leftArrow.interactable = false;
        }

        StartCoroutine("Slide");
    }



    //-------
    //sliding animation

    IEnumerator Slide()
    {

        float eTime = 0f;
        float startPosition = content.anchoredPosition.x;

        float track = targetPosition - startPosition;

        while (eTime < animationTime)
        {

            float xPos = curve.Evaluate(eTime / animationTime) * track;

            content.anchoredPosition = new Vector2(startPosition + xPos, content.anchoredPosition.y);

            yield return null;

            eTime += Time.deltaTime;

        }

        content.anchoredPosition = new Vector2(targetPosition, content.anchoredPosition.y);
    }




}
