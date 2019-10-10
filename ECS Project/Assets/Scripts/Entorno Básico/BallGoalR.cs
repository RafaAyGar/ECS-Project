using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoalR : MonoBehaviour
{
    public bool p2Finished = false;
    GameObject player2;
    Rigidbody p2Rigidbody;
    BallGoalL bGL;

    // Start is called before the first frame update
    void Start()
    {
        player2 = GameObject.Find("Player2");
        p2Rigidbody = player2.GetComponent<Rigidbody>();
        bGL = GameObject.FindObjectOfType<BallGoalL>();
    }

    private void Update()
    {
        if (bGL.p1Finished && p2Finished) UnityEditor.EditorApplication.isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player2")
        {
            p2Finished = true;
            p2Rigidbody.useGravity = false;
            p2Rigidbody.velocity = Vector3.zero;
            player2.GetComponent<ControllerP2>().enabled = false;
        }
    }
}
