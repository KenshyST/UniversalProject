using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformRotation : MonoBehaviour
{
    public string targetTag = "RotatingObject"; // Etiqueta de los objetos que deseas rotar
    public float rotationSpeed = 30.0f;

    private GameObject[] objectsToRotate;

    void Start()
    {
        // Encuentra todos los objetos con la etiqueta especificada
        objectsToRotate = GameObject.FindGameObjectsWithTag(targetTag);
    }

    void Update()
    {
        // Rotar todos los objetos en la lista
        foreach (GameObject obj in objectsToRotate)
        {
            obj.transform.Rotate(Vector3.up, rotationSpeed);
        }
    }
}
