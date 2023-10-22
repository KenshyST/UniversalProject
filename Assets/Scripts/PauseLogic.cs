using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private GameObject UIPausa;

    [SerializeField] private GameObject UIJugador;

    [SerializeField] private bool controladorPausa;
    void Start()
    {
        
    }

    private void Awake()
    {
        UIJugador = GameObject.Find("UIJugador");

    }

    // Update is called once per frame
    void Update()
    {
        PauseWithCancelButton();
    }

    void PauseWithCancelButton()
    {
        if (Input.GetButtonDown("Cancel") && !controladorPausa)
        {
            UIPausa.SetActive(true);
            controladorPausa = true;
            Time.timeScale = 1;
            UIJugador.SetActive(false);
        }
        else if (Input.GetButtonDown("Cancel") && controladorPausa)
        {
            UIPausa.SetActive(false);
            controladorPausa = false;
            Time.timeScale = 1;
            UIJugador.SetActive(true);
            
        }
    }
    
    public void continueGameWithButton()
    {
        UIPausa.SetActive(false);
        UIJugador.SetActive(true);
        controladorPausa = false;
        Time.timeScale = 1;
    }
}
