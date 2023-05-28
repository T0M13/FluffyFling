using System;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("References")]
    //[SerializeField] private UIManager uIManager;
    [Header("Game")]
    [SerializeField] private bool gameOver;
    //[Header("Player Stats")]
    [Header("Save/Load")]
    [SerializeField] private SaveComponent saveBehaviour;
    [SerializeField] private LoadComponent loadBehaviour;

    public Action OnReload;

    public bool GameOver { get => gameOver; set => gameOver = value; }

    public Action OnSave;
    public Action OnLoad;
    public Action OnGameOver;


    private void OnEnable()
    {
        OnSave += Save;
        OnLoad += Load;
        OnGameOver += CallGameOver;

        OnReload += ReloadSlingshot;
    }

    private void OnDisable()
    {
        OnSave -= Save;
        OnLoad -= Load;
        OnGameOver -= CallGameOver;

        OnReload -= ReloadSlingshot;

    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Load();
        Save();

        GameOver = false;

        //if (uIManager == null)
        //    uIManager = FindObjectOfType<UIManager>();
    }

    private void ReloadSlingshot()
    {

    }

    private void CallGameOver()
    {
        Save();
        GameOver = true;
        Debug.Log("Game Over");
    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting");
    }

    private void Save()
    {
        saveBehaviour.Save();
    }

    private void Load()
    {
        loadBehaviour.Load();
    }
}
