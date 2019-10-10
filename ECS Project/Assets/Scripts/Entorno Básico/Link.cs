using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject a, b;
    public float distanceVectorX;
    public float distance;
    public float angle;
    Vector3 distanceVector;

    // Start is called before the first frame update
    void Start()
    {
        a = FindObjectOfType<Controller>().gameObject;
        b = FindObjectOfType<ControllerP2>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(a.transform.position, b.transform.position);
        distanceVector = (b.transform.position - a.transform.position);
        angle = Vector3.Angle(distanceVector, Vector3.right);
        if(a.transform.position.y < b.transform.position.y) transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        else transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -angle);
        transform.localScale = new Vector3(distance, 1 - distance/19, 1);
        if (transform.localScale.y < 0)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log("¡¡El enlace se ha roto!!");
        }
        transform.localPosition = new Vector3((b.transform.position.x - a.transform.position.x)/2, (b.transform.position.y - a.transform.position.y) / 2, transform.localPosition.z);
    }
}
