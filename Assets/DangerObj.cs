using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerObj : MonoBehaviour
{
    private GameManager gameManager;
    private Renderer myRenderer;
    private Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool enableDangerObjects = gameManager.GetGameState() == GameState.STARTED ||
            gameManager.GetGameState() == GameState.COMPLETED;
        myRenderer.enabled = enableDangerObjects;
        myCollider.enabled = enableDangerObjects;
    }
}
