using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Player player;
    private bool gameOver;
    private float startTime;
    private int currentGoalIdx;

    protected float goalStartTime;

    // Start is called before the first frame update
    void Start()
    {
        var uiGO = Instantiate(whatIsUI);
        statusText = GameObject.Find("StatusText").GetComponent<TMPro.TextMeshProUGUI>();
        winText = GameObject.Find("WinText").GetComponent<TMPro.TextMeshProUGUI>();
        nextGoalText = GameObject.Find("NextGoalText").GetComponent<TMPro.TextMeshProUGUI>();
        timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        winText.enabled = false;
        player = FindObjectOfType<Player>();

        startTime = Time.time;

        goals = new List<HuippisGoal>(FindObjectsOfType<HuippisGoal>());
        ActivateRandomNext();
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

    void ActivateRandomNext()
    {
        GetCurrentGoal().Deactivate();
        int index = Random.Range(0, goals.Count);
        currentGoalIdx = index;
        goals[currentGoalIdx].Activate();
        goalStartTime = Time.fixedUnscaledTime;

        nextGoalText.text = string.Format("Next up:\n{0}", goals[currentGoalIdx].goalName);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetRemainingTime() <= 0)
        {
            gameOver = true;
        }

        if (gameOver)
        {
            winText.text = string.Format(
                "Game over!\nYour score: {0}",
                score
            );
            statusText.enabled = false;
            winText.enabled = true;
        }
        else
        {
            statusText.text = string.Format(
                "Next Activity: {0}\nScore: {1}\nMultiplier: {2}\nKarhu: {3}\nKulta Katriina: {4}",
                GetCurrentGoal().goalName,
                score,
                multiplier,
                player.getAttractiveObjCount(),
                player.getRepulsiveObjCount()
            );
            timerText.text = GetRemainingTime().ToString();
        }

        float timeSinceNewGoal = Time.fixedUnscaledTime - goalStartTime;
        nextGoalText.enabled = timeSinceNewGoal < 5.0f && ((timeSinceNewGoal % 0.2f) < 0.1f);
    }

    private float GetRemainingTime()
    {
        return totalTime - (Time.time - startTime);
    }
}
