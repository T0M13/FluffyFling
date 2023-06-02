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
    [SerializeField] private FollowCamera mainCam;
    [SerializeField] private Slingshot slingshot;
    [Header("Game")]
    [SerializeField] private bool gameOver;
    [SerializeField] private int score;
    //[Header("Player Stats")]
    [Header("Save/Load")]
    [SerializeField] private SaveComponent saveBehaviour;
    [SerializeField] private LoadComponent loadBehaviour;

    public Action OnReload;

    public bool GameOver { get => gameOver; set => gameOver = value; }

    public Action OnSave;
    public Action OnLoad;
    public Action OnGameOver;


    //public Action<GameObject, BirdState> OnThrow;
    //public Action<GameObject, BirdState> OnLoaded;
    public Action<GameObject> OnDeath;

    private void OnEnable()
    {
        OnSave += Save;
        OnLoad += Load;
        OnGameOver += CallGameOver;

        //OnThrow += ThrowBird;
        //OnLoaded += LoadedBird;
        OnDeath += DiedBird;
    }

    private void OnDisable()
    {
        OnSave -= Save;
        OnLoad -= Load;
        OnGameOver -= CallGameOver;

        //OnThrow -= ThrowBird;
        //OnLoaded -= LoadedBird;
        OnDeath -= DiedBird;

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

        mainCam = Camera.main.GetComponent<FollowCamera>();
        slingshot = FindObjectOfType<Slingshot>();
        GameOver = false;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    private void CallGameOver()
    {
        Save();
        GameOver = true;
        Debug.Log("Game Over");
    }

    //private void ThrowBird(GameObject gameObject, BirdState state)
    //{
    //    Debug.Log("I am being thrown");
    //    mainCam.SetTarget(gameObject, state);
    //}

    //private void LoadedBird(GameObject gameObject, BirdState state)
    //{
    //    Debug.Log("I am being loaded");
    //            mainCam.SetTarget(gameObject, state);
    //}
    private void DiedBird(GameObject gameObject)
    {
        gameObject = slingshot.GetCurrentBird();
        if (gameObject == null)
        {
            gameObject = slingshot.gameObject;
        }
        mainCam.SetTarget(gameObject, SlingshotState.Idle);
        Debug.Log("I am being killed");
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
