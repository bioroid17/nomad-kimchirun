using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Intro,
    Playing,
    Dead,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State = GameState.Intro;

    public float PlayStartTime;

    public int Lives = 3;

    [Header("References")]
    public GameObject IntroUI;
    public GameObject DeadUI;
    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldSpawner;

    public Player PlayerScript;

    public TMP_Text ScoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IntroUI.SetActive(true);
    }

    float CalculateScore()
    {
        return Time.time - PlayStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        int currentHighScore = PlayerPrefs.GetInt("HighScore");
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public float CalculateGameSpeed()
    {
        if (State != GameState.Playing)
        {
            return 5f;
        }
        float speed = 5f + (0.5f * Mathf.FloorToInt(CalculateScore() / 10f));
        return Mathf.Min(speed, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Playing)
        {
            ScoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }
        else if (State == GameState.Dead)
        {
            ScoreText.text = "High Score: " + GetHighScore();
        }
        if (State == GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            State = GameState.Playing;
            IntroUI.SetActive(false);
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldSpawner.SetActive(true);
            PlayStartTime = Time.time;
        }
        if (State == GameState.Playing && Lives == 0)
        {
            PlayerScript.KillPlayer();
            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldSpawner.SetActive(false);
            DeadUI.SetActive(true);
            State = GameState.Dead;
            SaveHighScore();
        }
        if (State == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
    }
}
