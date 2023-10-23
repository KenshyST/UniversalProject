using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private GameObject UIPausa;

    [SerializeField] private GameObject UIJugador;

    [SerializeField] private GameObject UIGameOver;
    [SerializeField] TextMeshProUGUI Mensaje;

    PlayerMovementGravity Player;



    [SerializeField] private bool controladorPausa;
    void Start()
    {
        Player = GetComponent<PlayerMovementGravity>();    
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

    public void GameOver()
    {
        Mensaje.text = "Gems: "+ Player.ScorePlayer+"/"+ GameObject.FindGameObjectsWithTag("Gems").Length + " \nRemaining lifes: " + Player.VidasPlayer;
        UIPausa.SetActive(false);
        UIJugador.SetActive(false);
        UIGameOver.SetActive(true);
    }

    public void ResetLevel()
    {
        // Carga una escena por nombre:
        SceneManager.LoadScene(1);

        // O carga una escena por índice:
        //SceneManager.LoadScene(1);

    }

    public void QuitLecel()
    {
        SceneManager.LoadScene(0);
    }
}
