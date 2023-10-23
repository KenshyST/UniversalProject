using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    public float rotationSpeedX = 10f;  // Velocidad de rotaci�n en el eje X
    public float rotationSpeedY = 10f;  // Velocidad de rotaci�n en el eje Y
    public float rotationSpeedZ = 10f;  // Velocidad de rotaci�n en el eje Z

    private void Update()
    {
        float rotationX = rotateX ? rotationSpeedX * Time.deltaTime : 0;
        float rotationY = rotateY ? rotationSpeedY * Time.deltaTime : 0;
        float rotationZ = rotateZ ? rotationSpeedZ * Time.deltaTime : 0;

        transform.Rotate(rotationX, rotationY, rotationZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(transform, true);
            //this.gameObject. ther
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.SetParent(null, true);
            //this.gameObject. ther
        }
    }
}
