using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CambiarEscena : MonoBehaviour
{
    public int escenas;
    public void Cambiar(int escena)
    {
        SceneManager.LoadScene(escena);
    }

    
}
