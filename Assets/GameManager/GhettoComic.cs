using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoComic : MonoBehaviour
{
    Vector3 initialPosition;
    protected GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * -5.0f, 0,0);
        if ((transform.position - initialPosition).x < -120) {
            transform.position = initialPosition;
        }

        if (gameManager.GetGameState() != GameState.BEFORE_START) {
            Destroy(gameObject);
        }
    }
}
