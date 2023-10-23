using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    public float openHeight = 5.0f; // Altura a la que la puerta debe abrirse
    public float closeHeight = 0.0f; // Altura a la que la puerta debe cerrarse (usualmente sería su posición inicial)
    public float speed = 2.0f; // Velocidad con la que se moverá la puerta
    public bool isOpening = false; // Controla si la puerta se está abriendo

    private Vector3 openPosition; // Posición final cuando la puerta está abierta
    private Vector3 closedPosition; // Posición inicial de la puerta

    [SerializeField] Button ButtonToOpen;

    void Start()
    {
        // Establece las posiciones de apertura y cierre basadas en la posición inicial de la puerta
        closedPosition = transform.position;
        openPosition = new Vector3(transform.position.x, closeHeight + openHeight, transform.position.z);
    }

    void Update()
    {
        isOpening = ButtonToOpen.isPressed;
        if (isOpening)
        {
            // Si la puerta se está abriendo, muévela hacia la posición de apertura
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
        }
        else
        {
            // Si la puerta se está cerrando, muévela hacia la posición de cierre
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
        }
    }

    // Función para abrir la puerta
    public void OpenDoor()
    {
        isOpening = true;
    }

    // Función para cerrar la puerta
    public void CloseDoor()
    {
        isOpening = false;
    }
}
