using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject target;
    public float xSpeed = 3.5f;
    float sensitivity = 17f;
    Camera Cam;
    

    float minFov = 35;
    float maxFov = 100;

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {


        //ZOOM

        float fov = Cam.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Cam.fieldOfView = fov;
    }
}
