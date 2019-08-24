using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<POI> POIs = new List<POI>();
    public List<HuippisGoal> route;

    private bool firstFrame = true;

    // Start is called before the first frame update
    void Start()
    {
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

    public void GoalCompleted() {
        route.RemoveAt(0);
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
    }
}
