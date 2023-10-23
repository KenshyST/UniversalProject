using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlatformLogic : MonoBehaviour
{
    public float delayBeforeFall = 2.0f; // Tiempo que espera antes de caer.
    private bool isPlayerOnTop = false;

    private void OnTriggerEnter(Collider other)
    {
        // Asumimos que el jugador tiene un tag llamado "Player".
        if (other.CompareTag("Player") && !isPlayerOnTop)
        {
            isPlayerOnTop = true;
            Invoke("StartFalling", delayBeforeFall); // Espera "delayBeforeFall" segundos y luego llama a StartFalling.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTop = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Destroy(gameObject); // Destruye la plataforma cuando colisiona con un objeto con el tag "Respawn".
        }
    }

    void StartFalling()
    {
        if (isPlayerOnTop) // Solo caer si el jugador todavía está encima.
        {
            GetComponent<PlanetPropierties>().enabled = false;
            GetComponent<OrbitLogic>().enabled = false;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>(); // Crea un nuevo componente Rigidbody para la plataforma.
        }
    }
}
