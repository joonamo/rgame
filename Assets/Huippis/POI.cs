using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    public float range = 10.0f;
    public float attract = 1.0f;
    public float directionMatch = 0.0f;
    public float exponent = 1.0f;

    protected GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.POIs.Add(this);
    }

    private void OnDestroy()
    {
        gameManager.POIs.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
