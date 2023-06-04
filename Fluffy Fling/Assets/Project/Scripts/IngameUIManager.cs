using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScoreUI;

    public TextMeshProUGUI CurrentScoreUI { get => currentScoreUI; set => currentScoreUI = value; }
}
