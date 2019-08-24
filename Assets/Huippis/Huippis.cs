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
    public float speedModifySpeed = 5.0f;
    protected float currentSpeedModify = 0.0f;

    protected int sightRayMask = 1 << 11;

    protected CharacterController charController;
    protected POI[] myPOI;

    public float avoidAngle = 20.0f;
    public float avoidSpeed = 30.0f;
    Quaternion leftQ;
    Quaternion rightQ;

    // Start is called before the first frame update
    void Start()
    {
        leftQ = Quaternion.Euler(0, avoidAngle, 0);
        rightQ = Quaternion.Euler(0, -avoidAngle, 0);

        myCollider = GetComponent<Collider>();
        myPOI = GetComponents<POI>();
        myCollider.enabled = false;

        charController = GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
        currentDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;

        speed += Random.Range(0.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flockDirection = currentDirection;
        float speedModify = 0.0f;
        foreach (POI poi in gameManager.POIs)
        {
            Vector3 diff = poi.transform.position - transform.position;

            if (poi.gameObject != gameObject && diff.magnitude < poi.range &&
                !Physics.Raycast(
                    transform.position, diff.normalized, diff.magnitude, sightRayMask))
            {
                float decay = 1.0f - Mathf.Pow((diff.magnitude / poi.range), poi.exponent);
                //Debug.DrawLine(transform.position, poi.transform.position); //Vector3.Lerp(transform.position, poi.transform.position, decay));
                flockDirection += diff.normalized * poi.attract * decay;
                flockDirection += poi.forward * poi.directionMatch * decay;
                speedModify += poi.speedup;
            }
        }
        if (flockDirection.magnitude > 0) {
            currentDirection = Vector3.Slerp(currentDirection, flockDirection, rotateSpeed * Time.deltaTime);
            currentDirection.y = 0;
            currentDirection.Normalize();
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, leftQ * currentDirection, out hit, 5.0f, sightRayMask)) {
            var str = 1.0f - hit.distance / 5.0f;
            currentDirection = Quaternion.Euler(0, -str * avoidAngle * Time.deltaTime * avoidSpeed, 0) * currentDirection;
        }
        if (Physics.Raycast(transform.position, rightQ * currentDirection, out hit, 5.0f, sightRayMask)) {
            var str = 1.0f - hit.distance / 5.0f;
            currentDirection = Quaternion.Euler(0, str * avoidAngle * Time.deltaTime * avoidSpeed, 0) * currentDirection;
        }
        Debug.DrawLine(transform.position, transform.position + leftQ * currentDirection * 5);
        Debug.DrawLine(transform.position, transform.position + rightQ * currentDirection * 5);

        currentSpeedModify = Mathf.Lerp(currentSpeedModify, speedModify, Time.deltaTime * speedModifySpeed);
        charController.SimpleMove(currentDirection * (speed + currentSpeedModify));
        foreach (POI poi in myPOI){
            poi.forward = currentDirection;
        }
        //Debug.DrawLine(transform.position, transform.position + currentDirection * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
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