using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThrowingTutorial : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public GameObject Container;
    public PickUpController PickUp;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if (PickUp.equipped)
        {
            Transform firstChildTransform = Container.transform.GetChild(0); // Obtiene el primer hijo del GameObject padre
            objectToThrow = firstChildTransform.gameObject; // Convierte el Transform del hijo en un GameObject

            if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0 && objectToThrow != null)
        {
            Throw();
        }
        }
        


        
    }

    public void Throw()
    {
        readyToThrow = false;

        // get rigidbody component del objeto en la mano
        Rigidbody projectileRb = objectToThrow.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // Desvincula el objeto de la mano del jugador
        objectToThrow.transform.parent = null;
        objectToThrow = null;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}