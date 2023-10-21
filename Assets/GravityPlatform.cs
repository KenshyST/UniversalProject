using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    [SerializeField]
    private float mass = 1.0f;

    public float Mass { get { return mass; } }

    private void Awake()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true; // Asegurarse de que el collider esté configurado como un trigger
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja una esfera en el editor para visualizar el área de influencia
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
