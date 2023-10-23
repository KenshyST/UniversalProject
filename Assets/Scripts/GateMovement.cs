using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMovement : MonoBehaviour
{
    public float openHeight = 5.0f; // Altura a la que la puerta debe abrirse
    public float closeHeight = 0.0f; // Altura a la que la puerta debe cerrarse (usualmente ser�a su posici�n inicial)
    public float speed = 2.0f; // Velocidad con la que se mover� la puerta
    public bool isOpening = false; // Controla si la puerta se est� abriendo

    private Vector3 openPosition; // Posici�n final cuando la puerta est� abierta
    private Vector3 closedPosition; // Posici�n inicial de la puerta

    [SerializeField] Button ButtonToOpen;

    void Start()
    {
        // Establece las posiciones de apertura y cierre basadas en la posici�n inicial de la puerta
        closedPosition = transform.position;
        openPosition = new Vector3(transform.position.x, closeHeight + openHeight, transform.position.z);
    }

    void Update()
    {
        isOpening = ButtonToOpen.isPressed;
        if (isOpening)
        {
            // Si la puerta se est� abriendo, mu�vela hacia la posici�n de apertura
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed * Time.deltaTime);
        }
        else
        {
            // Si la puerta se est� cerrando, mu�vela hacia la posici�n de cierre
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
        }
    }

    // Funci�n para abrir la puerta
    public void OpenDoor()
    {
        isOpening = true;
    }

    // Funci�n para cerrar la puerta
    public void CloseDoor()
    {
        isOpening = false;
    }
}
