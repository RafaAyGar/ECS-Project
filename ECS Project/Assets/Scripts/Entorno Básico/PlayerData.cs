using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerData : MonoBehaviour
{
    public bool centralLimit = false, roofLimit = false;
    public bool onFloor;
    public bool canJump = true;
    public bool circularMovingConditions = false, movingCircular = false, canMoveCircular = true;
    public bool mirrorMode = true;
    public float rightMove = -8;
    public float leftMove = 8;
    public Color initialColor;
    public Color initialLightColor;
    public Vector3 jumpForce = new Vector3(0, 4650, 0);
    public GameObject otherPlayer;
    public Light luz;

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform") && (other.enabled = true))
        {
            onFloor = true;
        }
        else if (other.tag == "CentralWall")
        {
            centralLimit = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform"))
        {
            onFloor = false;
        }
        else if (other.tag == "CentralWall")
        {
            centralLimit = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Floor") || (other.tag == "Platform") && (other.enabled = true))
        {
            onFloor = true;
        }
    }

    IEnumerator jumpRetard()
    {
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

    IEnumerator circularMovingRetard()
    {
        yield return new WaitForSeconds(2);
        canMoveCircular = true;
    }
}

class PlayersManager : ComponentSystem
{
    protected override void OnStartRunning()
    {
        Entities.ForEach((PlayerData data, Transform transform, Rigidbody rigidbody) =>
        {
            data.initialLightColor = data.luz.color;
        });

    }

    protected override void OnUpdate()
    {
        Entities.ForEach((PlayerData data, Transform transform, Rigidbody rigidbody) =>
        {
            //Mantiene la esfera sin rotar, ya que había un fallo en el que al hacer el movimiento circular rotaba sensiblemente.
            transform.rotation = Quaternion.identity;

        //Cambia el modo de espejo
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            data.mirrorMode = !data.mirrorMode;
        }
        //TENSAR ENLACE
        if (Input.GetKey(KeyCode.LeftShift) && data.onFloor)
        {
            rigidbody.velocity = Vector3.zero;
        }
        //MOVIMIENTO CIRCULAR
        data.circularMovingConditions = Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && ((!data.otherPlayer.GetComponent<PlayerData>().centralLimit && !data.otherPlayer.GetComponent<PlayerData>().roofLimit) || data.otherPlayer.GetComponent<PlayerData>().onFloor) && data.canMoveCircular && data.onFloor;
        if (data.movingCircular && !data.circularMovingConditions)
        {
            data.canMoveCircular = false;
            data.StartCoroutine("circularMovingRetard");
        }
        if (data.circularMovingConditions)
        {
            data.movingCircular = true;
            data.otherPlayer.GetComponent<Rigidbody>().useGravity = false;
            data.otherPlayer.transform.RotateAround(transform.position, Vector3.forward, 40 * Time.deltaTime);
        }
        else
        {
            data.otherPlayer.GetComponent<Rigidbody>().useGravity = true;
            data.movingCircular = false;
        }
        //SALTO
        if (Input.GetKeyDown(KeyCode.Space) && data.onFloor && data.canJump)
        {
            rigidbody.AddForce(data.jumpForce);
            data.canJump = false;
            data.StartCoroutine("jumpRetard");
        }
        //MOVER DERECHA
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift) && (!data.centralLimit || data.onFloor))
        {
            if (data.onFloor) rigidbody.velocity = new Vector3(data.leftMove, rigidbody.velocity.y, rigidbody.velocity.z);
            else rigidbody.velocity = new Vector3(data.leftMove / 2, rigidbody.velocity.y, rigidbody.velocity.z);
        }
        //CUANDO DEJAMOS DE MOVERNOS LA BOLA RECIBE INERCIA
        if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) && data.onFloor && !Input.GetKey(KeyCode.LeftShift))
        {
            rigidbody.velocity = rigidbody.velocity / 3;
        }
        //ACTIVAR LA GRAVEDAD PARA LA BOLA QUE ACABA DE SALIR DEL MOVIMIENTO CIRCULAR
        if (!data.circularMovingConditions)
        {
            data.otherPlayer.GetComponent<Rigidbody>().useGravity = true;
        }
        //MOVIMIENTO IZQUIERDO
        if (Input.GetKey(KeyCode.A) && data.onFloor)
        {
            if (data.onFloor) rigidbody.velocity = new Vector3(data.rightMove, rigidbody.velocity.y, rigidbody.velocity.z);
            else rigidbody.velocity = new Vector3(data.rightMove / 2, rigidbody.velocity.y, rigidbody.velocity.z);
        }
        });
    }
}

