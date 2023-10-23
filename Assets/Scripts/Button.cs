using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool isPressed = false; // Para saber si el bot�n est� presionado

    private Vector3 initialPosition; // Posici�n inicial del bot�n
    public float pressDistance = 0.1f; // Distancia que se mover� el bot�n cuando sea presionado

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed) // Aseg�rate de que tu jugador tenga el tag "Player"
        {
            PressButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPressed)
        {
            ReleaseButton();
        }
    }

    private void PressButton()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - pressDistance, transform.position.z);
        isPressed = true;

        // Aqu� puedes agregar cualquier otro c�digo para cuando el bot�n se presione
        // Por ejemplo, activar una puerta, encender una luz, etc.
    }

    private void ReleaseButton()
    {
        transform.position = initialPosition;
        isPressed = false;

        // Aqu� puedes agregar cualquier otro c�digo para cuando el bot�n se libere
    }
}
