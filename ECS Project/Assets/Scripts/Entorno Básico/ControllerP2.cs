using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerP2 : MonoBehaviour
{
    //--Public Variables ------------------------------------------------------------
    public bool leftLimit = false, roofLimit = false;
    public bool onFloor;
    public bool canJump = true;
    public bool circularMovingConditions = false, movingCircular = false, canMoveCircular = true;

    //--Private Variables ------------------------------------------------------------
    bool mirrorMode = true;
    float rightMove = -8;
    float leftMove = 8;
    public Color initialColor;
    public Color initialLightColor;
    Vector3 jumpForce = new Vector3(0, 5000, 0);
    Controller player1Controller;
    public Light luz;
    BallGoalL ballGoal;

    //--Unity Functions -------------------------------------------------------------
    void Start()
    {
        luz = gameObject.GetComponentInChildren<Light>();
        initialColor = gameObject.GetComponent<Renderer>().material.color;
        player1Controller = FindObjectOfType<Controller>();
        initialLightColor = luz.color;
        ballGoal = GameObject.FindObjectOfType<BallGoalL>();
    }

    // Update is called once per frame
    void Update()
    {
        //Para evitar errores de rotaciones no intencionadas
        transform.rotation = Quaternion.identity;

        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && onFloor && canJump)
        {
            Jump();
        }
        //Tensar Enlace
        if (Input.GetKey(KeyCode.LeftShift) && onFloor)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        //Movimiento Derecho
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift) && (!leftLimit || onFloor))
        {
            Move(rightMove);
        }
        //Movimiento Izquierdo
        if (Input.GetKey(KeyCode.A) && !movingCircular)
        {
            Move(leftMove);
        }
        //Disminuye la inercia cuando se deja de mover la bola
        if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) && onFloor && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        //Movimiento Circular. Comprobaciones
        if (movingCircular) circularMovingConditions = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && ((!player1Controller.rightLimit && !player1Controller.roofLimit) || player1Controller.onFloor) && canMoveCircular && onFloor && !player1Controller.mirrorMode;
        else circularMovingConditions = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && ((!player1Controller.rightLimit && !player1Controller.roofLimit) && player1Controller.onFloor) && canMoveCircular && onFloor && !player1Controller.mirrorMode;
        //Movimiento Circular
        if (canMoveCircular && circularMovingConditions)
        {
            StartCoroutine("CircularMovingTime");
            player1Controller.GetComponent<Rigidbody>().useGravity = false;
            player1Controller.transform.RotateAround(transform.position, Vector3.forward, -6f / (Vector3.Distance(transform.position, player1Controller.transform.position)/2));
        }
        else
        {
            movingCircular = false;
            if(!ballGoal.p1Finished) player1Controller.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform") && (other.enabled = true))
        {
            onFloor = true;
        }
        else if (other.tag == "CentralWall")
        {
            leftLimit = true;
        }
        else if (other.tag == "Roof")
        {
            roofLimit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform"))
        {
            onFloor = false;
        }
        else if (other.tag == "CentralWall")
        {
            leftLimit = false;
        }
        else if (other.tag == "Roof")
        {
            roofLimit = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform") && (other.enabled = true))
        {
            onFloor = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    //--My Functions -----------------------------------------------------------------------------

    void Jump()
    {
        GetComponent<Rigidbody>().AddForce(jumpForce);
        canJump = false;
        StartCoroutine("jumpRetard");
    }
    private void Move(float direction)
    {
        if (onFloor) GetComponent<Rigidbody>().velocity = new Vector3(direction, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        else GetComponent<Rigidbody>().velocity = new Vector3(direction / 2, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
    }

    //-Coroutines -------------------------------------
    private IEnumerator jumpRetard()
    {
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

    private IEnumerator CircularMovingTime()
    {
        movingCircular = true;
        yield return new WaitForSeconds(1.5f);
        canMoveCircular = false;
        movingCircular = false;
        yield return new WaitForSeconds(3);
        canMoveCircular = true;
    }
}
