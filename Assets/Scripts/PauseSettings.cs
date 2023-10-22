using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
    public AudioMixer theMixer;

    public TextMeshProUGUI musicLabel, soundEffectLabel;

    public Slider musicSlider, soundEffectSlider;

    public void Start()
    {
        float vol = 0f;
        theMixer.GetFloat("MusicVol", out vol);
        musicSlider.value = vol;
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        theMixer.GetFloat("SoundEffectVol", out vol);
        soundEffectSlider.value = vol;
        soundEffectLabel.text = Mathf.RoundToInt(soundEffectSlider.value + 80).ToString();

    }

    public void SetMusicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSoundEffectVolume()
    {
        soundEffectLabel.text = Mathf.RoundToInt(soundEffectSlider.value + 80).ToString();
        theMixer.SetFloat("SoundEffectVol", soundEffectSlider.value);
        PlayerPrefs.SetFloat("SoundEffectVol", musicSlider.value);
    }



}
