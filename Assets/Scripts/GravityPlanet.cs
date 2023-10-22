using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanet : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravityStrength = 9.81f;  // Fuerza gravitacional de este planeta

    private void OnTriggerEnter(Collider other)
    {
        PlayerGravity player = other.GetComponent<PlayerGravity>();
        if (player != null)
        {
            player.currentPlanetCore = transform; // Establece este planeta como el núcleo gravitacional actual para el jugador
            player.gravity = -gravityStrength; // Actualiza la fuerza gravitacional para el jugador
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerGravity player = other.GetComponent<PlayerGravity>();
        if (player != null && player.currentPlanetCore == transform)
        {
            // Si el jugador sale del campo gravitacional, restablece la gravedad y el núcleo por defecto
            player.currentPlanetCore = null;
            player.gravity = -9.81f;
        }
    }
}
