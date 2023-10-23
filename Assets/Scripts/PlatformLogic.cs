using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    public Transform pointA; // Punto inicial
    public Transform pointB; // Punto final
    public float speed = 5.0f; // Velocidad de la plataforma

    private Vector3 nextPosition; // La siguiente posición hacia la que la plataforma se dirigirá
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        nextPosition = pointB.position; // Al inicio, la plataforma se moverá hacia el punto B
    }

    void Update()
    {
        MovePlatform();

        // Si la plataforma llega a uno de los puntos, cambia la dirección
        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {
            nextPosition = pointA.position;
        }
        else if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
        {
            nextPosition = pointB.position;
        }
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(transform, true);
            //this.gameObject. ther
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null, true);
            //this.gameObject. ther
        }
    }
}
