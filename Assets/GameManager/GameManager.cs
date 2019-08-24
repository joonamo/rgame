using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<POI> POIs = new List<POI>();
    public List<HuippisGoal> route;

    public int score = 0;
    public int multiplier = 1;

    public GameObject whatIsUI;
    protected TMPro.TextMeshProUGUI statusText;

    private Player player;
    private bool firstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        var uiGO = Instantiate(whatIsUI);
        statusText = GameObject.Find("StatusText").GetComponent<TMPro.TextMeshProUGUI>();
        player = FindObjectOfType<Player>();

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
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame) {
            route[0].Activate();
            firstFrame = false;
        }

        statusText.text = string.Format(
            "Next Activity: {0}\nScore: {1}\nMultiplier: {2}\nKarhu: {3}\nKulta Katriina: {4}",
            GetCurrentGoal().goalName,
            score,
            multiplier,
            player.getAttractiveObjCount(),
            player.getRepulsiveObjCount()
        );
    }
}
