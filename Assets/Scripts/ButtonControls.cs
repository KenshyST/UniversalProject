using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent (typeof(Animator))]
public class ButtonControls : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("El sonido que se reproducirá cuando se haga clic en el botón.")]
    public AudioSource buttonAudioSource;

    [Header("Controlador De Menu principal")]
    [Tooltip("Añadir el menu principal")]
    public GameObject mainMenuUI;

    [Header("Controlador De Menu Credits")]
    [Tooltip("Añadir el menu Credits")]
    public GameObject creditsMenuUi;

    public void Start()
    {
        buttonAudioSource = GetComponent<AudioSource>();
        mainMenuUI.SetActive(true);
        creditsMenuUi.SetActive(false);
        
    }

    public void Awake()
    {
        
        mainMenuUI = GameObject.Find("UIMainMenu");
        creditsMenuUi = GameObject.Find("UICredits");
        
    }
    public void HoverSound()
    {
        buttonAudioSource.Play();
    }
    
    public void changeUi(int proximaEscena)
    {
        switch (proximaEscena)
        {
            case 0:
                
                creditsMenuUi.SetActive(false);
                mainMenuUI.SetActive(true);
            break;
                
            case 1:
                mainMenuUI.SetActive(false);
                creditsMenuUi.SetActive(true);
                break;

            default:
            Debug.Log("Valor no válido");
            break;
        }
    }
    
}
