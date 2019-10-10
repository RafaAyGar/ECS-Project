using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoalL : MonoBehaviour
{
    public bool p1Finished = false;
    GameObject player;
    Rigidbody p1Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        p1Rigidbody = player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player1")
        {
            p1Finished = true;
            p1Rigidbody.useGravity = false;
            p1Rigidbody.velocity = Vector3.zero;
            player.GetComponent<Controller>().enabled = false;
        }
    }
}
