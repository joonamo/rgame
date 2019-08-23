using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuippisGoal : MonoBehaviour
{
    protected Collider myCollider;
    protected GameManager gameManager;

    protected int huippisEntered;
    protected bool active;

    public GameObject whatIsHuippis;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        //myCollider.enabled = false;
        gameManager = FindObjectOfType<GameManager>();
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
    }

    public void Deactivate() {
        myCollider.enabled = false;
        for (int i = huippisEntered; i > 0; --i) {
            Instantiate(whatIsHuippis, transform.position, transform.rotation);
        }
    }
}
