using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("General")]
    public float gravity = -9.81f;
    private Vector3 velocity;

    [Header("References")]
    public Camera playerCamera;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public bool doubleJumpEnabled = false;
    private bool canDoubleJump = true;
    bool readyToJump;
    public float jumpCooldown;

    [Header("Rotation")]
    public float rotationSensitivity = 2.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveInput = Vector3.zero;

    private CharacterController characterController;

    private float verticalVelocity = 0;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    public float internalCounterCoyoteTime;

    private CinemachineFreeLook freeLookCam;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        freeLookCam = FindObjectOfType<CinemachineFreeLook>();
        readyToJump = true;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            canDoubleJump = true;
            velocity.y = -1.86f;
            internalCounterCoyoteTime = coyoteTime;
        }
        else
        {
            internalCounterCoyoteTime -= Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        float speed = Input.GetButton("Sprint") ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && (characterController.isGrounded || (internalCounterCoyoteTime > 0 && readyToJump)))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump && doubleJumpEnabled)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            canDoubleJump = false;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}