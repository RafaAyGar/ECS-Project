using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActions : MonoBehaviour
{
    GameObject player;
    float maxSize = 10, size = 0.5f;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    private void Update()
    {
        if(size < maxSize)
        {
            player.transform.localScale = new Vector3(0.5f, size, 0.5f);
            size += size*Time.deltaTime;
        }
    }
}
