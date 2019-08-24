using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuippisGoal : MonoBehaviour
{
    protected Collider myCollider;
    protected Renderer[] myRenderers;
    protected GameManager gameManager;
    protected POI myPOI;

    protected int huippisEntered;
    protected bool active;

    public int attractiveObjGain;
    public int repulsiveObjGain;

    public GameObject whatIsHuippis;
    public GameObject whatIsTitle;

    public string goalName = "Sauna";
    private TMPro.TextMeshPro titleMesh;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRenderers = GetComponents<Renderer>();
        myPOI = GetComponent<POI>();
        //myCollider.enabled = false;
        gameManager = FindObjectOfType<GameManager>();

        titleMesh = Instantiate(
            whatIsTitle,
            transform.position + Vector3.up * (transform.localScale.y + 2),
            Quaternion.identity)
            .GetComponent<TMPro.TextMeshPro>();
        titleMesh.text = goalName;

        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active && gameManager.GetCurrentGoal() && gameManager.GetCurrentGoal().GetInstanceID() == this.GetInstanceID())
        {
            Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (!active)
        {
            return;
        }

        if (other.tag == "Huippis")
        {
            Destroy(other.gameObject);
            ++huippisEntered;
            gameManager.addScore();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButtonDown("Jump"))
        {
            CompleteGoal();
            RechargeSupplies(other.GetComponent<Player>());
        }
    }

    public void Activate() {
        Debug.Log("Activated");
        active = true;
        myCollider.enabled = true;
        myPOI.attract = 0.5f;
        foreach (var r in myRenderers)
        {
            r.enabled = true;
        }
        titleMesh.enabled = true;
    }

    public void Deactivate()
    {
        active = false;
        myCollider.enabled = false;
        myPOI.attract = 0.0f;
        foreach (var r in myRenderers)
        {
            r.enabled = false;
        }
        titleMesh.enabled = false;
    }

    public void CompleteGoal()
    {
        Deactivate();
        for (int i = huippisEntered; i > 0; --i) {
            Instantiate(whatIsHuippis, transform.position, transform.rotation);
        }
        gameManager.GoalCompleted();
        
    }

    private void RechargeSupplies(Player player)
    {
        player.RechargeInventory(attractiveObjGain, repulsiveObjGain);
    }
}
