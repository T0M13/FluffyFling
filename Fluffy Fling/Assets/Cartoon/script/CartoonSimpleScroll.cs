using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script calculates the size of the content 

public class CartoonSimpleScroll : MonoBehaviour
{
    private RectTransform rect;
    private GridLayoutGroup grid;


    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
    }

    void OnEnable()
    {
        int chNumb = transform.childCount;

        float hSize = grid.cellSize.y * Mathf.Ceil((float)chNumb / grid.constraintCount) + grid.spacing.y * (Mathf.Ceil((float)chNumb / grid.constraintCount) - 1f);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, hSize);
    }
}
