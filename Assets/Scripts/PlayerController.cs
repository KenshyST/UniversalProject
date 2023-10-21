using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("General")]
    public float gravity = -9.81f;

    [Header("References")]
    public Camera playerCamera;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 8f;
    bool readyToJump;
    public float jumpCooldown;


    [Header("Rotation")]
    public float rotationSensitivity = 2.0f;

    private Vector3 moveDirection = Vector3.zero;
    
    private Vector3 moveInput = Vector3.zero;

    private CharacterController characterController;

    private float verticalVelocity = 0;

    [Header("Rotation")]
    public float coyoteTime = 0.2f;
    [SerializeField] private float internalCounterCoyoteTime;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        readyToJump = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float speed = Input.GetButton("Sprint") ? runSpeed : walkSpeed;

        // Si está en el suelo, resetea el temporizador coyote
        if (characterController.isGrounded)
        {
            internalCounterCoyoteTime = coyoteTime;
        }
        else
        {
            // Si está en el aire, decrementa el temporizador coyote
            internalCounterCoyoteTime -= Time.deltaTime;
        }

        // Control de salto
        if (characterController.isGrounded || (internalCounterCoyoteTime > 0))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                readyToJump = false;
                verticalVelocity = jumpForce;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveInput = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput) * speed);
        moveDirection = new Vector3(moveInput.x, verticalVelocity, moveInput.z);

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.Rotate(Vector3.right * mouseY);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
