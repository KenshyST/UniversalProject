using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLogic : MonoBehaviour
{
    public int playerScore;
    public int PlayerLifes;
    [SerializeField] private PlayerMovementGravity Player;
    [SerializeField] private TextMeshProUGUI UITexto;
    [SerializeField] private TextMeshProUGUI UILife;
    [SerializeField] private TextMeshProUGUI historyText;
    [SerializeField] private GameObject BackgroundHistory;

    private void Start()
    {
        
        UITexto = GameObject.Find("ScoreUI").GetComponent<TextMeshProUGUI>();
        UpdateLifesUI();
    }

    public void UpdateScoreUI()
    {
        Player.ScorePlayer = playerScore;
        UITexto.text = "Crystals: " + playerScore;
    }
    public void UpdateLifesUI()
    {
        PlayerLifes = Player.VidasPlayer;
        UILife.text = "Lifes " + PlayerLifes;
    }

    public void IncreaseScore()
    {
        playerScore += 1;
        UpdateScoreUI();
    }

    public void DecreaseScore(int minus)
    {
        playerScore -= minus; // Deberías restar, no sumar.
        UpdateScoreUI();
    }

    public void sectionOfHistory(float time, string mensaje, AudioClip narration)
    {

        StartCoroutine(showHistory(time,mensaje,narration));
        

    }

    IEnumerator showHistory(float time, string mensaje, AudioClip narration)
    {
        BackgroundHistory.SetActive(true);
        historyText.text = mensaje;
        yield return new WaitForSeconds(time); 
        BackgroundHistory.SetActive(false);
        historyText.text = "";
    }

}
