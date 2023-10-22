using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.down;
    public Transform currentPlanetCore = null;
    public float defaultGravity = 9.81f;
    public float gravity = 9.81f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactiva la gravedad predeterminada del Rigidbody.
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (currentPlanetCore != null)
        {
            gravityDirection = (transform.position - currentPlanetCore.position).normalized;
            transform.up = Vector3.Lerp(transform.up, -gravityDirection, 0.1f); // Interpolación para una rotación más suave.
        }
        else
        {
            gravityDirection = Vector3.down;
        }

        rb.AddForce(gravityDirection * gravity, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        GravityPlanet planet = other.GetComponent<GravityPlanet>();
        if (planet)
        {
            currentPlanetCore = planet.transform;
            gravity = planet.gravityStrength;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GravityPlanet planet = other.GetComponent<GravityPlanet>();
        if (planet && currentPlanetCore == planet.transform)
        {
            currentPlanetCore = null;
            gravity = defaultGravity;
        }
    }
}
