using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider mainAudioSlider, musicSlider, sfxSlider;

    void Start()
    {
        SetMainAudioVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMainAudioVolume()
    {
        float volume = mainAudioSlider.value;
        audioMixer.SetFloat("mainAudio", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }

    public void ClickSFX()
    {
        AudioManager.instance.ClickSFX();
    }
}
