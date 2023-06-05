using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameUIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI currentScoreUI;
    [SerializeField] private GameObject inGameCanvas;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI endScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject noStar;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;


    public TextMeshProUGUI CurrentScoreUI { get => currentScoreUI; set => currentScoreUI = value; }

    public void ShowGameOverScreen(int score, int stars, bool victory)
    {
        inGameCanvas.SetActive(false);

        endScoreUI.text = score.ToString();

        switch (stars)
        {
            case 0:
                noStar.SetActive(true);
                break;
            case 1:
                oneStar.SetActive(true);
                break;
            case 2:
                twoStar.SetActive(true);
                break;
            case 3:
                threeStar.SetActive(true);
                break;
        }

        victoryPanel.SetActive(victory);

        if (victory)
        {
            gameOverText.text = "Victory";
        }
        else
        {
            gameOverText.text = "Game Over";
        }

        gameOverCanvas.SetActive(true);
    }
}
