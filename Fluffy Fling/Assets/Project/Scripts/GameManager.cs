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
    [SerializeField] private IngameUIManager ingameUIManager;
    [SerializeField] private FollowCamera mainCam;
    [SerializeField] private Slingshot slingshot;
    [SerializeField] private BirdsManager birdsManager;
    [Header("Game")]
    [SerializeField] private bool gameOver;
    [SerializeField] private bool victory;
    [SerializeField] private int score;
    //[Header("Player Stats")]
    [Header("Enemies Left")]
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private float waitCheckBirds;
    //[SerializeField] private float timer;
    //[SerializeField] private float timerCooldown = 5f;
    private Coroutine birdChecking;
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

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mainCam = Camera.main.GetComponent<FollowCamera>();
        slingshot = FindObjectOfType<Slingshot>();
        birdsManager = FindObjectOfType<BirdsManager>();
        GameOver = false;
    }

    public void AddScore(int score)
    {
        this.score += score;
        ingameUIManager.CurrentScoreUI.text = this.score.ToString();
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    private void Update()
    {
        //if (victory || gameOver) return;
        //timer -= Time.deltaTime;
        //if (timer <= 0)
        //{
        //    CheckEnemiesLeft();
        //    timer = timerCooldown;
        //}
    }

    public void CheckEnemiesLeft()
    {
        if (victory || gameOver) return;
        if (enemies.Count <= 0)
        {
            Debug.Log("Victory");
            victory = true;
        }
        else
        {
            if (slingshot.GetCurrentBird() == null)
                birdChecking = StartCoroutine(CheckBirdsLeft());
        }
    }

    private IEnumerator CheckBirdsLeft()
    {
        yield return new WaitForSeconds(waitCheckBirds);

        if (birdsManager.SpawnedBirds.Count <= 0)
        {
            Debug.Log("Game Over");
            gameOver = true;
        }
        else
        {
            //Birds Left to shoot
        }
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
        CheckEnemiesLeft();
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
