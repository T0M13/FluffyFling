using System;
using System.Collections;
using System.Collections.Generic;
using tomi.SaveSystem;
using tomi.Audio;
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
    [SerializeField] private bool paused;
    [SerializeField] private int score;
    [SerializeField] private int stars;
    [Header("Scores Needed")]
    [SerializeField] private int oneStarScore;
    [SerializeField] private int twoStarScore;
    [SerializeField] private int threeStarScore;
    [SerializeField] private int birdScore;
    [Header("Level Index")]
    [SerializeField] private int levelIndex;
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
    [Header("Loaded Stats")]
    [SerializeField] private int savedScore;
    [SerializeField] private int savedStars;

    public Action OnReload;

    public bool GameOver { get => gameOver; set => gameOver = value; }
    public bool Paused { get => paused; set => paused = value; }
    public bool Victory { get => victory; set => victory = value; }

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

        ResumeGame();
        Load();
        Save();

        Time.timeScale = 1;
        levelIndex = SceneManager.GetActiveScene().buildIndex - 2;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mainCam = Camera.main.GetComponent<FollowCamera>();
        slingshot = FindObjectOfType<Slingshot>();
        birdsManager = FindObjectOfType<BirdsManager>();

        GameOver = false;
        Victory = false;
        paused = false;
        stars = 0;
        score = 0;


        if (levelIndex < 0) return;
        savedScore = SaveData.PlayerProfile.scores[levelIndex];
        savedStars = SaveData.PlayerProfile.stars[levelIndex];

        if (!AudioManager.instance) return;
        AudioManager.instance.ResumeSong();
        AudioManager.instance.Play("forestMusic");

    }

    public void AddScore(int score)
    {
        if (victory || gameOver) return;
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
        if (Victory || gameOver) return;
        if (enemies.Count <= 0)
        {
            Debug.Log("Victory");
            Victory = true;
            CallGameOver();
        }
        else
        {
            if (slingshot.GetCurrentBird() == null || birdsManager.SpawnedBirds.Count <= 0)
                birdChecking = StartCoroutine(NoBirdsLeft());
            else
            {
                //Birds Left to shoot
            }

        }
    }

    private IEnumerator NoBirdsLeft()
    {
        yield return new WaitForSeconds(waitCheckBirds);
        if (birdsManager.SpawnedBirds.Count <= 0)
        {
            Debug.Log("Game Over");
            gameOver = true;
            CallGameOver();
        }
    }

    private void CalculateBirdsLeft()
    {
        if (birdsManager.SpawnedBirds.Count > 0)
        {
            foreach (GameObject bird in birdsManager.SpawnedBirds)
            {
                Bird birdScript = bird.GetComponent<Bird>();
                birdScript.SetScoreOn(birdScore);
                this.score += birdScore;
                ingameUIManager.CurrentScoreUI.text = this.score.ToString();
            }
        }
    }

    private void CallGameOver()
    {
        StartCoroutine(CallGameOverLogic());
    }

    private IEnumerator CallGameOverLogic()
    {
        yield return new WaitForSeconds(.5f);
        CalculateBirdsLeft();
        yield return new WaitForSeconds(2f);
        CalculateStars();
        CalculateScores();
        Save();
        StartCoroutine(CallGameOverUI(score, stars, victory));
    }

    private IEnumerator CallGameOverUI(int score, int stars, bool victory)
    {
        yield return new WaitForSeconds(2.5f);
        ingameUIManager.ShowGameOverScreen(score, stars, victory);
        Time.timeScale = 0;
    }

    private void CalculateScores()
    {
        if (SaveData.PlayerProfile.scores[levelIndex] < score)
            SaveData.PlayerProfile.scores[levelIndex] = score;
        else
            return;

    }

    private void CalculateStars()
    {
        if (gameOver && !victory)
        {
            stars = 0;
            if (SaveData.PlayerProfile.stars[levelIndex] > 0) return;
            SaveData.PlayerProfile.stars[levelIndex] = 0;
            return;
        }

        if (score < oneStarScore)
        {
            stars = 0;
        }
        if (score >= oneStarScore)
        {
            stars = 1;

            if (score >= twoStarScore)
            {
                stars = 2;

                if (score >= threeStarScore)
                {
                    stars = 3;
                }
            }
        }

        if (SaveData.PlayerProfile.stars[levelIndex] > stars) return;
        SaveData.PlayerProfile.stars[levelIndex] = stars;
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        Paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Paused = false;
    }

    public void StartGameMainMenu()
    {
        SceneManager.LoadScene(1); // Level Select
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting");
    }

    public void NextGame()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCount)
        {
            SceneManager.LoadScene(1); //LevelSelect
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    private void Save()
    {

        saveBehaviour.Save();
        savedScore = SaveData.PlayerProfile.scores[levelIndex];
        savedStars = SaveData.PlayerProfile.stars[levelIndex];
    }

    private void Load()
    {
        loadBehaviour.Load();
    }

    public void OpenMilkStudios()
    {
        Application.OpenURL("https://milk-studios.at");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
