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
    Vector3 originalRotation;

    void Start()
    {
        originalRotation = transform.eulerAngles;
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
        // Si el jugador está en el suelo, no es necesario calcular la gravedad de otros planetas
        if (isGrounded)
        {
            Physics.gravity = GravityDirection * 9.81f;
            return;
        }

        // Determinar el planeta más cercano y su distancia
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

        // Si el jugador no está en la órbita de un planeta, usar gravedad hacia abajo
        if (closestPlanetIndex == -1 || !isInOrbit)
        {
            GravityDirection = Vector3.down;
            Physics.gravity = GravityDirection * 9.81f;

            // Obtiene la rotación actual en ángulos de Euler
            Vector3 currentEuler = transform.eulerAngles;

            // Interpola solo en los ejes X y Z
            currentEuler.x = Mathf.LerpAngle(currentEuler.x, originalRotation.x, RotationSpeed * Time.deltaTime);
            currentEuler.z = Mathf.LerpAngle(currentEuler.z, originalRotation.z, RotationSpeed * Time.deltaTime);

            // Aplica la nueva rotación
            transform.eulerAngles = currentEuler;

            return;
        }

        IndexPlanet = closestPlanetIndex;
        GravityDirection = (Planets[IndexPlanet].transform.position - transform.position).normalized;

        // Si el objeto NO es esférico, utilizamos raycasting sofisticado
        if (!Planets[IndexPlanet].GetComponent<PlanetPropierties>().isSpherical)
        {
            int numberOfRays = 100;
            Vector3 averageNormal = Vector3.zero;
            int hitCount = 0;

            for (int i = 0; i < numberOfRays; i++)
            {
                Vector3 dir = Random.onUnitSphere;

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

            if (hitCount > 0)
            {
                GravityDirection = -averageNormal / hitCount;
            }
        }

        Physics.gravity = GravityDirection * 11F;

        // Asegurarse de que el jugador esté de pie
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, -GravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
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
