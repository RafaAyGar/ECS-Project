using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    float averagePlayerYPos = 0.0f;
    Transform p1, p2;
    Vector3 movementSpeed = new Vector3(0, 0.5f, 0);
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.FindObjectOfType<Controller>().transform;
        p2 = GameObject.FindObjectOfType<ControllerP2>().transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        averagePlayerYPos = (p1.position.y + p2.position.y) / 2;

        if(averagePlayerYPos > transform.position.y)
        {
            if(movementSpeed.y < 2.5f) movementSpeed += new Vector3(0, 0.01f, 0);
        }
        else
        {

        }

        rigidbody.velocity = movementSpeed;
    }
}
