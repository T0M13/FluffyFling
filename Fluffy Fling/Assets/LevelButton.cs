using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public Image[] stars = new Image[3];
    public TextMeshProUGUI score;
    public int index;

    public void LoadLevel()
    {
        SceneManager.LoadScene(index);
    }

    public void SetStars(int stars, Sprite star)
    {
        for (int i = 0; i < stars; i++)
        {
            this.stars[i].sprite = star;
        }
    }

    public void SetScore(int score)
    {
        if (score <= 0)
        {
            this.score.text = "";
        }
        else
        {
            this.score.text = score.ToString();
        }
    }

}
