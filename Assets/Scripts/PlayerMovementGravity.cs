using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGravity : MonoBehaviour
{
    private GameObject[] Planets;
    public int IndexPlanet; // Planeta mas cercano
    public float MovingSpeed;
    public float RunningSpeed;
    public float RotationSpeed;
    private Rigidbody rb;
    private bool isRunning;
    public float JumpForce;
    public bool isGrounded;
    private bool canDoubleJump;
    public bool allowDoubleJump = true; // Activa o desactiva el doble salto.
    public float coyoteTime = 0.2f; // Tiempo que puede saltar después de dejar el suelo.
    private float coyoteCounter;
    Vector3 OldGravity;
    public float peso = 70;
    public bool isInOrbit;

    void Start()
    {
        OldGravity = Physics.gravity;
        IndexPlanet = 0;
        rb = GetComponent<Rigidbody>();
        isRunning = false;
        canDoubleJump = false;
    }

    void Update()
    {
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        SetGravity();
        Movement();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float actualSpeed = 0.0f;

        // Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            actualSpeed = RunningSpeed;
        }
        else
        {
            isRunning = false;
            actualSpeed = MovingSpeed;
        }

        if (vertical >= 0.2) { transform.Translate(new Vector3(0, 0, actualSpeed * Time.deltaTime)); }
        if (vertical <= -0.2) { transform.Translate(new Vector3(0, 0, -actualSpeed * Time.deltaTime)); }
        if (horizontal <= -0.2) { transform.Rotate(new Vector3(0, -RotationSpeed * Time.deltaTime, 0)); }
        if (horizontal >= 0.2) { transform.Rotate(new Vector3(0, RotationSpeed * Time.deltaTime, 0)); }

        // Jumping and Double Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || coyoteCounter > 0)
            {
                rb.AddForce(transform.up * JumpForce);
                isGrounded = false;
                coyoteCounter = 0;
                if (allowDoubleJump)
                {
                    canDoubleJump = true;
                }
            }
            else if (canDoubleJump)
            {
                rb.AddForce(transform.up * JumpForce);
                canDoubleJump = false;
            }
        }

        // Coyote time
        if (!isGrounded)
        {
            coyoteCounter -= Time.deltaTime;
        }
    }


    Vector3 GravityDirection;
    private void SetGravity()
    {
        // Si el jugador está en el suelo, solo sentimos la gravedad de ese planeta específico.
        if (isGrounded || isInOrbit)
        {
            ApplyPlanetGravity(IndexPlanet);
            return;
        }



        // Buscar el planeta más cercano.
        float menorDistancia = float.MaxValue;
        int closestPlanetIndex = -1;
        for (int i = 0; i < Planets.Length; i++)
        {
            float distancia = Vector3.Distance(transform.position, Planets[i].transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                closestPlanetIndex = i;
            }
        }

        // Si el planeta más cercano está dentro de un rango específico (por ejemplo, 100f), 
        // cambiamos la gravedad basada en ese planeta. De lo contrario, gravedad normal.
        if (menorDistancia <= 5f)
        {
            IndexPlanet = closestPlanetIndex;
            ApplyPlanetGravity(IndexPlanet);
        }
        else
        {
            Physics.gravity = Vector3.down * 9.81f; // Gravedad normal.
        }
    }

    private void ApplyPlanetGravity(int planetIndex)
    {
        GravityDirection = (Planets[planetIndex].transform.position - transform.position).normalized;

        if (!Planets[planetIndex].GetComponent<PlanetPropierties>().isSpherical)
        {
            GravityDirection = CalculateNonSphericalGravityDirection();
        }

        Physics.gravity = GravityDirection * 9.81f;
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, -GravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
    }

    private Vector3 CalculateNonSphericalGravityDirection()
    {
        Vector3 averageNormal = Vector3.zero;
        int hitCount = 0;
        Vector3[] rayDirections = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.up,
        Vector3.down
    };

        foreach (Vector3 dir in rayDirections)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, 100f))
            {
                if (hit.collider.gameObject.CompareTag("Planet"))
                {
                    averageNormal += hit.normal;
                    hitCount++;
                }
            }
        }

        return hitCount > 0 ? -averageNormal / hitCount : Vector3.down;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Planet"))
        {
            isGrounded = true;
            canDoubleJump = false;
            coyoteCounter = coyoteTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Planet"))
        {
            isGrounded = false;
        }
    }
}
