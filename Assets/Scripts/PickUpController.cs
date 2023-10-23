using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    //public ProjectileGun gunScript;
    public PlayerMovementGravity movement;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public GameObject Mira;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public float trowForwardForce, trowUpwardForce;


    public bool equipped;
    public static bool slotFull;
    private bool IsActive = false;

    private void Start()
    {
        //Setup
        if (!equipped)
        {
           // gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;

            Mira.SetActive(false);
            movement.ReleaseObject();
        }
        if (equipped)
        {
           // gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        //Check if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        //Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();

        if (equipped && Input.GetKeyDown(KeyCode.Mouse1) && !IsActive)
        {
            Mira.SetActive(true);
            movement.GrabObject();
            IsActive = true;            
        }
        else if(equipped && Input.GetKeyDown(KeyCode.Mouse1) && IsActive)
        {
            Mira.SetActive(false);
            movement.ReleaseObject();
            IsActive = false;
        }

        if (equipped && Input.GetKeyDown(KeyCode.Mouse0) && IsActive) Throw();

    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        //gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        //gunScript.enabled = false;

        Mira.SetActive(false);
        movement.ReleaseObject();

        IsActive = false;
    }

    public void Throw()
    {
        equipped = false;
        slotFull = false;

        // Desvincula el objeto de la
        // mano del jugador
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        // get rigidbody component del objeto en la mano
        Rigidbody projectileRb = gameObject.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = fpsCam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - gunContainer.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * trowForwardForce + transform.up * trowUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        Mira.SetActive(false);
        movement.ReleaseObject();

        IsActive = false;
    }
}
