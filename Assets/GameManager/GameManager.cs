using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<POI> POIs = new List<POI>();
    public List<HuippisGoal> route;

    public int score = 0;
    public int multiplier = 1;
    public float totalTime;

    public GameObject whatIsUI;
    protected TMPro.TextMeshProUGUI statusText;
    protected TMPro.TextMeshProUGUI winText;
    protected TMPro.TextMeshProUGUI timerText;

    private Player player;
    private bool firstFrame = true;
    private bool gameOver;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        var uiGO = Instantiate(whatIsUI);
        statusText = GameObject.Find("StatusText").GetComponent<TMPro.TextMeshProUGUI>();
        winText = GameObject.Find("WinText").GetComponent<TMPro.TextMeshProUGUI>();
        timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        winText.enabled = false;
        player = FindObjectOfType<Player>();

        startTime = Time.time;

        var goals = new List<HuippisGoal>(FindObjectsOfType<HuippisGoal>());
        while (goals.Count > 0 && route.Count < 5) {
            int index = Random.Range(0, goals.Count);
            route.Add(goals[index]);
            goals.RemoveAt(index);
        }

        route[0].Activate();
    }

    public HuippisGoal GetCurrentGoal() {
        return route.Count > 0 ? route[0] : null ;
    }

    public void addScore() {
        score += multiplier;
    }

    public void GoalCompleted() {
        route[0].Deactivate();
        route.RemoveAt(0);
        multiplier = score;
        if (route.Count > 0) {
            route[0].Activate();
        }
        else
        {
            gameOver = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame) {
            route[0].Activate();
            firstFrame = false;
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
            timerText.text = ((totalTime - (Time.time - startTime))).ToString();
        }
    }
}
