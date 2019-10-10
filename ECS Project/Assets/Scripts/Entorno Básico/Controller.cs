using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //--Public Variables ------------------------------------------------------------
    public bool rightLimit = false, roofLimit = false;
    public bool onFloor;
    public bool canJump = true;
    public bool circularMovingConditions = false, movingCircular = false, canMoveCircular = true;
    public bool mirrorMode = true;

    //--Private Variables ------------------------------------------------------------
    float rightMove = -8;
    float leftMove = 8;
    Color initialColor;
    Color initialLightColor;
    Vector3 jumpForce = new Vector3(0, 5000, 0);
    ControllerP2 player2;
    Light luz;
    BallGoalR ballGoal;

    //--Unity Functions -------------------------------------------------------------
    void Start()
    {
        luz =           gameObject.GetComponentInChildren<Light>();
        initialColor =  gameObject.GetComponent<Renderer>().material.color;
        player2 =       FindObjectOfType<ControllerP2>();
        ballGoal = GameObject.FindObjectOfType<BallGoalR>();
        //initialLightColor = luz.color;
    }

    // Update is called once per frame
    void Update()
    {
        //Para evitar errores de rotaciones no intencionadas
        transform.rotation = Quaternion.identity;
        //Cambiar Modo de espejo
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mirrorMode = !mirrorMode;
            luz.color = player2.luz.color;
        }
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
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift) && (!rightLimit || onFloor))
        {
            Move(leftMove);
        }
        //Movimiento Izquierdo
        if (Input.GetKey(KeyCode.A) && !movingCircular)
        {
            Move(rightMove);
        }
        //Disminuye la inercia cuando se deja de mover la bola
        if((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) && onFloor && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        //Movimiento Circular. Comprobaciones
        if (movingCircular) circularMovingConditions = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && ((!player2.leftLimit && !player2.roofLimit) || player2.onFloor ) && onFloor && mirrorMode;
        else circularMovingConditions = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && ((!player2.roofLimit) && player2.onFloor) && canMoveCircular && onFloor && mirrorMode;
        //Movimiento Circular
        if (circularMovingConditions)
        {
            StartCoroutine("CircularMovingTime");
            player2.GetComponent<Rigidbody>().useGravity = false;
            player2.transform.RotateAround(transform.position, Vector3.forward, 6f / (Vector3.Distance(transform.position, player2.transform.position)/2));
        }
        else
        {
            movingCircular = false;
            //if(!ballGoal.p2Finished) player2.GetComponent<Rigidbody>().useGravity = true;
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
            rightLimit = true;
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
            rightLimit = false;
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
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
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
        canMoveCircular = false;
        yield return new WaitForSeconds(2);
        movingCircular = false;
        yield return new WaitForSeconds(3);
        canMoveCircular = true;
    }
}
