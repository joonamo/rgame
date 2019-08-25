using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public float totalRotationTime;
    public AnimationCurve myCurve;
    public int pageIdx;

    private float rotationStartTime = -1;
    private bool rotatingRight;

    public void Turn()
    {
        if (!IsRotationOn())
        {
            rotationStartTime = Time.time;
            rotatingRight = false;
        }
    }

    public void TurnBack()
    {
        if (!IsRotationOn())
        {
            rotationStartTime = Time.time;
            rotatingRight = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRotationOn())
        {
            if (rotatingRight)
            {
                ContinueRotatingRight();
            } else
            {
                ContinueRotatingLeft();
            }   
        }

        if ((Time.time - rotationStartTime) / totalRotationTime > 1)
        {
            rotationStartTime = -1;
            rotatingRight = false;
        }
    }

    public bool IsRotationOn()
    {
        return rotationStartTime >= 0 && (Time.time - rotationStartTime) / totalRotationTime < 1;
    }

    private void ContinueRotatingLeft()
    {
        var eulerRotation = Quaternion.ToEulerAngles(this.transform.rotation);
        var currRotationPercentage = myCurve.Evaluate((Time.time - rotationStartTime) / totalRotationTime);

        this.transform.rotation = Quaternion.Euler(0, 180 * currRotationPercentage, 0);

        // makes sure pages do not sink in each other
        float idxAdjustment = -Mathf.Sin(eulerRotation.y / 2) * pageIdx * 0.05f;
        this.transform.position = new Vector3(
            1.5f * (Mathf.Cos(eulerRotation.y) - 1),
            0,
            -1.5f * Mathf.Sin(eulerRotation.y) + idxAdjustment
        );
    }


    private void ContinueRotatingRight()
    {
        var eulerRotation = Quaternion.ToEulerAngles(this.transform.rotation);
        var currRotationPercentage = myCurve.Evaluate((Time.time - rotationStartTime) / totalRotationTime);

        this.transform.rotation = Quaternion.Euler(0, 180 - 180 * currRotationPercentage, 0);

        // makes sure pages do not sink in each other
        float idxAdjustment = Mathf.Sin(eulerRotation.y / 2) * pageIdx * 0.05f;
        this.transform.position = new Vector3(
            -1.5f + 1.5f * (Mathf.Cos(eulerRotation.y)),
            0,
            -1.5f * Mathf.Sin(eulerRotation.y) + idxAdjustment
        );
    }
}
