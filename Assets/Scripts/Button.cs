using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool isPressed = false; // Para saber si el botón está presionado
    public bool isHold = false; // Para saber si el botón está presionado

    private Vector3 initialPosition; // Posición inicial del botón
    public float pressDistance = 0.1f; // Distancia que se moverá el botón cuando sea presionado

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed) // Asegúrate de que tu jugador tenga el tag "Player"
        {
            PressButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPressed)
        {
            if (!isHold) { ReleaseButton(); }
        }
    }

    private void PressButton()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - pressDistance, transform.position.z);
        isPressed = true;

        // Aquí puedes agregar cualquier otro código para cuando el botón se presione
        // Por ejemplo, activar una puerta, encender una luz, etc.
    }

    private void ReleaseButton()
    {
        transform.position = initialPosition;
        isPressed = false;

        // Aquí puedes agregar cualquier otro código para cuando el botón se libere
    }
}
