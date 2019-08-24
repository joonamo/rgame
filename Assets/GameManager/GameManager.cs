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

    private bool firstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
        var uiGO = Instantiate(whatIsUI);
        statusText = GameObject.Find("StatusText").GetComponent<TMPro.TextMeshProUGUI>();

        var goals = new List<HuippisGoal>(FindObjectsOfType<HuippisGoal>());
        while (goals.Count > 0 && route.Count < 5) {
            int index = Random.Range(0, goals.Count);
            route.Add(goals[index]);
            goals.RemoveAt(index);
        }
    }

    public HuippisGoal GetCurrentGoal() {
        return route.Count > 0 ? route[0] : null ;
    }

    public void addScore() {
        print("Add score");
        score += multiplier;
    }

    public void GoalCompleted() {
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
            "Next Activity: {0}\nScore: {1}\nMultiplier: {2}",
            GetCurrentGoal().goalName, score, multiplier);
    }
}
