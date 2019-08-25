using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<POI> POIs = new List<POI>();
    public List<HuippisGoal> goals;

    public int score = 0;
    public int multiplier = 1;
    public float totalTime;

    public GameObject whatIsUI;
    protected TMPro.TextMeshProUGUI statusText;
    protected TMPro.TextMeshProUGUI winText;
    protected TMPro.TextMeshProUGUI nextGoalText;
    protected TMPro.TextMeshProUGUI timerText;
    protected Image logoImage;

    protected GameState gameState = GameState.BEFORE_START;

    private Player player;
    private float startTime;
    private int currentGoalIdx;

    protected float goalStartTime;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        multiplier = 1;
        gameState = GameState.BEFORE_START;

        var uiGO = Instantiate(whatIsUI);
        statusText = GameObject.Find("StatusText").GetComponent<TMPro.TextMeshProUGUI>();
        winText = GameObject.Find("WinText").GetComponent<TMPro.TextMeshProUGUI>();
        nextGoalText = GameObject.Find("NextGoalText").GetComponent<TMPro.TextMeshProUGUI>();
        timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        logoImage = GameObject.Find("Logo").GetComponent<Image>();
        winText.enabled = false;
        player = FindObjectOfType<Player>();

        goals = new List<HuippisGoal>(FindObjectsOfType<HuippisGoal>());

        winText.text = "Press Activate to start!";
        winText.enabled = true;
    }

    public HuippisGoal GetCurrentGoal() {
        return goals[currentGoalIdx];
    }

    public void addScore() {
        score += multiplier;
    }

    public void GoalCompleted() {
        multiplier = score;
        ActivateRandomNext();
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    void ActivateRandomNext()
    {
        GetCurrentGoal().Deactivate();
        int index = Random.Range(0, goals.Count);
        while (index == currentGoalIdx) {
            index = Random.Range(0, goals.Count);
        }
        currentGoalIdx = index;
        goals[currentGoalIdx].Activate();
        goalStartTime = Time.fixedUnscaledTime;

        nextGoalText.text = string.Format("Next up:\n{0}", goals[currentGoalIdx].goalName);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (gameState != GameState.STARTED)
        {
            return;
        }

        if (GetRemainingTime() <= 0)
        {
            HandleGameOver();
        }

        statusText.text = string.Format(
                "Next Activity: {0}\nScore: {1}\nMultiplier: {2}\nKarhu: {3}\nKulta Katriina: {4}",
                GetCurrentGoal().goalName,
                score,
                multiplier,
                player.getAttractiveObjCount(),
                player.getRepulsiveObjCount()
            );
        timerText.text = GetRemainingTime().ToString("0.00");

        float timeSinceNewGoal = Time.fixedUnscaledTime - goalStartTime;
        nextGoalText.enabled = timeSinceNewGoal < 5.0f && ((timeSinceNewGoal % 0.2f) < 0.1f);
    }

    private void HandleGameOver()
    {
        gameState = GameState.COMPLETED;
        Time.timeScale = 0.0f;

        var highScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (highScore < score)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        winText.text = string.Format(
                "Game over!\nYour score: {0}\nHigh score: {1}\n\n Press Activate to start again",
                score,
                highScore
            );
        statusText.enabled = false;
        winText.enabled = true;
    }

    private void HandleInput()
    {
        if (gameState == GameState.BEFORE_START && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Start the game");
            gameState = GameState.STARTED;
            startTime = Time.time;
            player.Init();
            ActivateRandomNext();
            logoImage.enabled = false;
            winText.enabled = false;
        }
        else if (gameState == GameState.COMPLETED && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1.0f;
        }
    }

    private float GetRemainingTime()
    {
        return totalTime - (Time.time - startTime);
    }
}
