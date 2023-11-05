using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataform : MonoBehaviour
{
    [Header("Object Interaction")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float throwNormalForce = 15f;
    [SerializeField] private float throwForwardForce = 15f;
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private LayerMask layerMask;

    [Header("Mouse Controls")]
    [SerializeField] private float scrollSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 30.0f;

    private GameObject heldObj;
    private Rigidbody heldObjRB;
    private Vector3 mouseMovementVector;
    private Transform holdArea = null;


    [Header("Handle Habilties")]
    [SerializeField] private bool rotateObj = false;
    [SerializeField] private bool throwObj = false;
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * pickupRange, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            TryPickUp();
        }
        else if (Input.GetMouseButtonUp(0) && heldObj != null)
        {
            Drop();
        }

        if (heldObj != null)
        {
            ResetObjectRotation();
            if (rotateObj)
            {
                RotateObject();
            }
           
            Move();
            ScrollObject(Input.GetAxis("Mouse ScrollWheel"));
            HandleMouseMovement();
        }

        
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, layerMask))
        {
            PickUp(hit.transform.gameObject);
        }
    }

    void PickUp(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>())
        {
            heldObj = obj;
            heldObjRB = obj.GetComponent <Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            CreateHoldArea();

            obj.transform.parent = holdArea;
            heldObj.gameObject.layer = 7; //Pasa a ser grabbed
        }
    }

    void CreateHoldArea()
    {
        holdArea = new GameObject("HoldArea").transform;
        holdArea.position = heldObj.transform.position;
        holdArea.rotation = heldObj.transform.rotation;
        holdArea.parent = transform;
    }

    void Drop()
    {
        heldObj.gameObject.layer = 6; //Pasa a ser grabbable
        //heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null;
        DestroyHoldArea();

        heldObjRB.AddForce(mouseMovementVector * throwNormalForce, ForceMode.Impulse);

        heldObj = null;
    }

    void DestroyHoldArea()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "HoldArea")
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    void Move()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickUpForce * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (throwObj)
            {
                Drop();
                heldObjRB.AddForce(transform.forward * throwForwardForce, ForceMode.Impulse);
            }
           
        }
    }

    void ScrollObject(float scroll)
    {
        Vector3 newPosition = holdArea.position + transform.forward * scroll * scrollSpeed;
        float distanceToObject = Vector3.Distance(transform.position, holdArea.position);

        if (scroll > 0 && distanceToObject + scroll * scrollSpeed < pickupRange)
        {
            holdArea.position = newPosition;
        }
        else if (scroll < 0 && distanceToObject + scroll * scrollSpeed > 1.5f)
        {
            holdArea.position = newPosition;
        }
    }

    void RotateObject()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        heldObj.transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }


    void HandleMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            mouseMovementVector = new Vector3(mouseX, mouseY, 0);
        }
    }

    void ResetObjectRotation()
    {
        heldObj.transform.rotation = Quaternion.identity; // Restablece la rotación a la rotación inicial
    }
}
