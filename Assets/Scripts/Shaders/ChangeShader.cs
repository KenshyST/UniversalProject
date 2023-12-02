using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeShader : MonoBehaviour
{
    public UniversalRendererData urp_data;
    private bool changeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        changeRenderer = false;
        urp_data.rendererFeatures[0].SetActive(changeRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)){

            changeRenderer = !changeRenderer;
            urp_data.rendererFeatures[0].SetActive(changeRenderer);

        }
   
       
    }
}
