using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuippisGoal : MonoBehaviour
{
    protected Collider myCollider;
    protected GameManager gameManager;
    protected POI myPOI;

    protected int huippisEntered;
    protected bool active;

    public GameObject whatIsHuippis;
    public GameObject whatIsTitle;

    public string goalName = "Sauna";
    private TMPro.TextMeshPro titleMesh;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myPOI = GetComponent<POI>();
        //myCollider.enabled = false;
        gameManager = FindObjectOfType<GameManager>();

        titleMesh = Instantiate(
            whatIsTitle,
            transform.position + Vector3.up * (transform.localScale.y + 2),
            Quaternion.identity)
            .GetComponent<TMPro.TextMeshPro>();
        titleMesh.text = goalName;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter!");
        if (other.tag == "Huippis")
        {
            Destroy(other.gameObject);
            ++huippisEntered;
        }
        else if (other.tag == "Player") {
            Deactivate();
        }
    }

    public void Activate() {
        myCollider.enabled = true;
        myPOI.attract = 0.5f;
    }

    public void Deactivate() {
        myCollider.enabled = false;
        for (int i = huippisEntered; i > 0; --i) {
            Instantiate(whatIsHuippis, transform.position, transform.rotation);
        }
        gameManager.GoalCompleted();
        myPOI.attract = 0.0f;
    }
}
