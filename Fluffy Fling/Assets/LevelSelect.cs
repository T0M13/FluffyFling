using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private LoadComponent loadBeh;
    [SerializeField] private SaveComponent saveBeh;
    [SerializeField] private Sprite emptyStar;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private LevelButton[] levels = new LevelButton[10];


    private void Start()
    {
        Load();
        Save();

        SetIndexes();
        SetStars();
        SetScore();
    }

    private void SetStars()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetStars(SaveData.PlayerProfile.stars[i], fullStar);
        }
    }

    private void SetScore()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetScore(SaveData.PlayerProfile.scores[i]);
        }
    }

    private void SetIndexes()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].index = i;
        }
    }

    private void Save()
    {
        saveBeh.Save();
    }

    private void Load()
    {
        loadBeh.Load();
    }
}
