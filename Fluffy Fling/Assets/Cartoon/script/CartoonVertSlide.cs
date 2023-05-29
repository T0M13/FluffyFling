using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script slides the list of buttons

public class CartoonVertSlide : MonoBehaviour
{
    public AnimationCurve curve;

    public float animationTime = 0.25f;

    public bool isScrollClamped = true;

    //public float velocity;

    private RectTransform element;

    private RectTransform content;
    private VerticalLayoutGroup vertGroup;

    private float startPos;
    private float targetPos;

    //-------

    void Awake()
    {
        content = GetComponent<RectTransform>();
        vertGroup = GetComponent<VerticalLayoutGroup>();
        element = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        //calculate content size
        int chNumb = gameObject.transform.childCount;
        content.sizeDelta = new Vector2(content.sizeDelta.x, element.sizeDelta.y * chNumb + vertGroup.spacing * (chNumb - 1));
    }

    //sliding to the closest position
    IEnumerator EndDragClamp()
    {

        float eTime = 0f;

        float track = targetPos - startPos;

        while (eTime < animationTime)
        {

            float yPos = curve.Evaluate(eTime / animationTime) * track;

            content.anchoredPosition = new Vector2(content.anchoredPosition.x, startPos + yPos);

            yield return null;

            eTime += Time.deltaTime;

        }

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, targetPos);

    }

    //calculate position
    public void OnDragEndClamp()
    {
        if (isScrollClamped) {
            startPos = content.anchoredPosition.y;

            float cSize = (vertGroup.spacing + element.sizeDelta.y);

            targetPos = Mathf.Round(startPos / cSize) * cSize;


            StartCoroutine("EndDragClamp");
        }
    }
}
