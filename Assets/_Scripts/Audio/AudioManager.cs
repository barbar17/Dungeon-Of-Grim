using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioSource backgroundMusicSource;
    [SerializeField]
    AudioSource sfxSource;
    [SerializeField]
    AudioSource normalizeSFXSource;
    [SerializeField]
    public AudioSource playerWalkSource;

    [Header("Music")]
    public AudioClip mainMenuMusic;
    public AudioClip dungeonMusic;
    public AudioClip finishedGameMusic;
    public AudioClip gameOverMusic;

    [Header("SFX")]
    public AudioClip clickSFX;
    public AudioClip typingSFX;

    [Header("PlayerSFX")]
    public AudioClip walkSFX;
    public AudioClip dashSFX;
    public AudioClip deathSFX;
    public AudioClip getHitSFX;
    public AudioClip[] attackSFX;
    public AudioClip[] powerAttackSFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerWalkSource.clip = walkSFX;
        PlayMusic(gameOverMusic);
    }

    public void PlayMusic(AudioClip audioClip)
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = audioClip;
        backgroundMusicSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    public void PlayNormalizeSFX(AudioClip audioClip)
    {
        normalizeSFXSource.PlayOneShot(audioClip);
    }

    //Global SFX
    public void TypingSFX()
    {
        PlaySFX(typingSFX);
    }

    public void ClickSFX()
    {
        PlaySFX(clickSFX);
    }

    //PlayerSFX
    public void PlayWalkSFX()
    {
        playerWalkSource.pitch = Random.Range(1.2f, 1.4f);
        playerWalkSource.Play();
    }

    public void StopWalkSFX()
    {
        playerWalkSource.Stop();
    }

    public void DashSFX()
    {
        PlayNormalizeSFX(dashSFX);
    }

    public void DeathSFX()
    {
        PlayNormalizeSFX(deathSFX);
    }

    public void GetHitSFX()
    {
        PlaySFX(getHitSFX);
    }
}
