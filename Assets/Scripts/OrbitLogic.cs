using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLogic : MonoBehaviour
{
    public bool isInOrbit;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Asegúrate de asignar este tag a los colliders de órbita
        {
            isInOrbit = true;
            if (!other.gameObject.GetComponent<PlayerMovementGravity>().isInOrbit)
            {
                other.gameObject.GetComponent<PlayerMovementGravity>().isInOrbit = isInOrbit;
                //other.gameObject.GetComponent<PlayerMovementGravity>().CurrentPlanet = this.gameObject;

            }
            
            //isGrounded = false; // Reset para que si se sale de la órbita vuelva a afectarle la gravedad normal al saltar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInOrbit = false;
            other.gameObject.GetComponent<PlayerMovementGravity>().isInOrbit = isInOrbit;
            //other.gameObject.GetComponent<PlayerMovementGravity>().CurrentPlanet = null;
        }
    }
}
