using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer theMixer;
    public void Start()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("SoundEffectVol", PlayerPrefs.GetFloat("SoundEffectVol"));
        }
    }
}
