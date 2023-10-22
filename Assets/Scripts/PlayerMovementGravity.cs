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

    

    private void SetGravity()
    {
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

        IndexPlanet = closestPlanetIndex;
        Vector3 gravityDirection = (Planets[IndexPlanet].transform.position - transform.position).normalized;

        // Si el objeto NO es esférico, utilizamos raycasts
        if (Planets[IndexPlanet].GetComponent<SphereCollider>() == null)
        {
            Vector3 averageNormal = Vector3.zero;
            int hitCount = 0;

            // Lanzamos raycasts en varias direcciones
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

            if (hitCount > 0)
            {
                gravityDirection = -averageNormal / hitCount;
            }
        }

        if (!isGrounded){

            float attractionStrength = 2.5f;
            Vector3 attractionForce = gravityDirection * attractionStrength;
            //rb.AddForce(attractionForce);
            Physics.gravity = gravityDirection * 9.81f;
        }
        else
        {
            Physics.gravity = gravityDirection * 9.81f;
        }
        
        //float attractionStrength = 2.5f;
        //Vector3 attractionForce = gravityDirection * attractionStrength;
        //rb.AddForce(attractionForce);

        // Asegurarse de que el jugador esté de pie
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
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
