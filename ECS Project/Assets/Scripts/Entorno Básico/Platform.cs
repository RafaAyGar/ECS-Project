using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //--Public variables --------------------------------------------------------
    public float correctHight;
    public float playerCorrectSize;
    public bool transition;

    //--Private variables -------------------------------------------------------
    Controller player;
    Collider platformCollider;

    //--Member Functions Unity --------------------------------------------------
    void Start()
    {
        correctHight = transform.position.y + (transform.localScale.y / 2);
        player = GameObject.FindObjectOfType<Controller>();
        platformCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if(player != null)
        {
            playerCorrectSize = (player.transform.position.y - (player.transform.localScale.y / 2));
            //Cuando supera la altura establecida de la plataforma, el collider se activa y se hace rígido.
            if (playerCorrectSize > correctHight && !transition)
            {
                platformCollider.enabled = true;
                StartCoroutine("Offset");
            }
            //Cuando el jugador esta debajo se desactiva el collider para que cuando la bola al saltar pase por este, no reconozca que esta tocando el suelo.
            else if (playerCorrectSize < correctHight - transform.localScale.y && !transition)
            {
                platformCollider.enabled = false;
            }
            //Cuando se pulsa S, se produce la transición, con la cual la bola se cae de la plataforma atravesándola
            if (Input.GetKeyDown(KeyCode.S) && player.onFloor && !platformCollider.isTrigger)
            {
                transition = true;
                platformCollider.isTrigger = true;
                StartCoroutine("waitTime");
            }
            //Mientras se realiza la transición, es decir, la bola cae de la plataforma, se le impide saltar, esto se ha hecho porque durante un pequeño lapso de
            //tiempo, si pulsabas el ESPACIO repetidas veces justo al pulsar S para descender, saltabas y la transición se cancel            if (transition) player.canJ;
        }
    }

    //--My Functions ------------------------------------------------------------
    IEnumerator waitTime()
    {
        //Al cabo de unos segundos que es lo que se calcula que la bola tarda en atravesar el collider de la plataforma, podemos devolver el objeto a la
        //normalidad y no causará problemas
        yield return new WaitForSeconds(0.5f);
        transition = false;
        player.canJump = true;
    }

    IEnumerator Offset()
    {
        yield return new WaitForSeconds(0.25f);
        platformCollider.isTrigger = false;
    }
}
