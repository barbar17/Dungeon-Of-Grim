using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource backgroundMusicSource;
    [SerializeField]
    AudioSource SFXSource;

    public AudioClip background;
    // public AudioClip ;
    // public AudioClip ;

    void Start()
    {
        backgroundMusicSource.clip = background;
        backgroundMusicSource.Play();
    }
}
