using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource enemySFXSource;
    [SerializeField]
    public AudioSource enemyWalkSource;

    public AudioClip walkSFX;
    public AudioClip attackSFX;
    public AudioClip getHitSFX;
    public AudioClip deathSFX;

    void Awake()
    {
        enemyWalkSource.clip = walkSFX;
    }

    public void PlaySFX(AudioClip audioClip)
    {
        enemySFXSource.PlayOneShot(audioClip);
    }

    public void AttackSFX()
    {
        PlaySFX(attackSFX);
    }

    public void GetHitSFX()
    {
        PlaySFX(getHitSFX);
    }

    public void PlayWalkSFX()
    {
        enemyWalkSource.Play();
    }

    public void StopWalkSFX()
    {
        enemyWalkSource.Stop();
    }

    public void DeathSFX()
    {
        AudioManager.instance.PlaySFX(deathSFX);
    }
}
