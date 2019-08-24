using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Transform CamTran = Camera.main.transform;
        Transform PlayerTransform = player.transform;

        CamTran.position = Vector3.Lerp(
            CamTran.position,
            new Vector3(
                PlayerTransform.position.x,
                PlayerTransform.position.y + 15,
                PlayerTransform.position.z - 12
            ), 3.0f * Time.deltaTime
        );
	}
}
