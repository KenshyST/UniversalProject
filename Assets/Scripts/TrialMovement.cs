using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrialMovement : MonoBehaviour
{
    [Header("General")]
    public float gravity = -9.81f;
    private Vector3 velocity;
    public bool isGrabbed = false;

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

    [Header("CineMachine")]
    private CinemachineFreeLook freeLookCam;
    private float lastTargetAngle;

    // Valores originales para restaurar la c�mara cuando se suelta el objeto
    private float originalTopRigHeight;
    private float originalMiddleRigRadius;

    // Valores deseados para la transici�n
    public float grabbedTopRigHeight = 7.1f;
    public float grabbedMiddleRigRadius = 6.26f;

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

        if (isGrabbed)
        {
            OrientPlayerToCamera();
            MoveWithCameraDirection(); 
            return;
        }

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
            lastTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, lastTargetAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0, lastTargetAngle, 0) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, lastTargetAngle, 0);
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

    private GravityPlatform currentPlatform = null;
    private void ApplyPlatformForce()
    {
        if (currentPlatform != null)
        {
            // Calcula la direcci�n hacia la plataforma
            Vector3 directionToPlatform = (currentPlatform.transform.position - transform.position).normalized;

            // Aplica una fuerza en direcci�n a la plataforma, basado en la masa de la plataforma.
            velocity += directionToPlatform * currentPlatform.Mass * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GravityPlatform platform = other.GetComponent<GravityPlatform>();
        if (platform != null)
        {
            currentPlatform = platform;
            gravity = -currentPlatform.Mass; // Cambia la gravedad basado en la masa de la plataforma
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GravityPlatform platform = other.GetComponent<GravityPlatform>();
        if (platform == currentPlatform)
        {
            currentPlatform = null;
            gravity = -9.81f; // Restablece la gravedad a su valor predeterminado
        }
    }

    public void OrientPlayerToCamera()
    {
        Vector3 cameraDirection = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        float targetAngle = Mathf.Atan2(cameraDirection.x, cameraDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void MoveWithCameraDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Obtenemos la direcci�n de movimiento relativa a la c�mara.
        Vector3 direction = playerCamera.transform.forward * vertical + playerCamera.transform.right * horizontal;
        direction.y = 0; // Mantenemos la componente y en 0 para no moverse hacia arriba o abajo.
        direction.Normalize(); // Normalizamos la direcci�n para que tenga una magnitud de 1.

        float speed = Input.GetButton("Sprint") ? runSpeed : walkSpeed;

        // Movemos al jugador en la direcci�n calculada.
        characterController.Move(direction * speed * Time.deltaTime);
    }


    public void GrabObject()
    {
        isGrabbed = true;
        velocity.y = 0; // Restablecer la velocidad vertical
    }

    public void ReleaseObject()
    {
        isGrabbed = false;
    }


}
