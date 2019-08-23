using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huippis : MonoBehaviour
{
    protected Collider myCollider;
    protected GameManager gameManager;
    protected Vector3 currentDirection;
    public float rotateSpeed = 10.0f;
    public float speed = 3.0f;

    protected CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;

        charController = GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
        currentDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flockDirection = Vector3.zero;
        foreach (POI poi in gameManager.POIs)
        {
            Vector3 diff = poi.transform.position - transform.position;
            if (diff.magnitude < poi.range)
            {
                float decay = 1.0f - Mathf.Pow((diff.magnitude / poi.range), poi.exponent);
                flockDirection += diff.normalized * poi.attract * decay;
                flockDirection += poi.transform.forward * poi.directionMatch * decay;
            }
        }
        currentDirection = Vector3.Slerp(currentDirection, flockDirection, rotateSpeed * Time.deltaTime);
        currentDirection.y = 0;
        currentDirection.Normalize();

        charController.SimpleMove(currentDirection * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Droppable item")
        {
            Destroy(other.gameObject);
        }
    }

    public void Activate()
    {
        myCollider.enabled = true;
    }

    public void Deactivate()
    {
        myCollider.enabled = false;
    }
}