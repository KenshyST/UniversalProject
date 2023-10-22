using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectibleLogic : MonoBehaviour
{
    public ScoreLogic scoreLogic;

    [SerializeField] private AudioSource audioCollectible;

    [Header("Configuraciones si es una memoria")]

    public string historiaMemoria = "";

    public AudioClip audioClip;

    public float timeWaitAudio;

    


    void Start()
    {
        scoreLogic = GameObject.Find("GameManager").GetComponent<ScoreLogic>();
        audioCollectible = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.transform.tag == "Player") && (gameObject.tag != "memory")) {
            scoreLogic.IncreaseScore();
            Destroy(gameObject);
        } else
        {

            StartCoroutine(waitTimer(timeWaitAudio));
        }
    }

    IEnumerator waitTimer(float time)
    {
        scoreLogic.sectionOfHistory(timeWaitAudio, historiaMemoria, audioClip);
        audioCollectible.clip = audioClip;
        audioCollectible.Play();
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


}
