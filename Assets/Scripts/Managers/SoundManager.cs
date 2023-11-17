using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDeathSounds;
    public AudioClip[] playerHitSounds;
    public AudioClip[] playerDeathSounds;
    public AudioClip[] playerShootSounds;

    public AudioClip[] gearCollectSounds;

    public AudioClip[] music;

    public AudioClip[] swordCollideSounds;
    public AudioClip[] swordSwingSounds;
    public AudioClip[] swordLootSounds;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayEnemyHitSound()
    {
        audioSource.PlayOneShot(enemyHitSounds[Random.Range(0, enemyHitSounds.Length)]);
    }

    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(enemyDeathSounds[Random.Range(0, enemyDeathSounds.Length)]);
    }

    public void PlayPlayerHitSound()
    {
        audioSource.PlayOneShot(playerHitSounds[Random.Range(0, playerHitSounds.Length)]);
    }

    public void PlayPlayerDeathSound()
    {
        audioSource.PlayOneShot(playerDeathSounds[Random.Range(0, playerDeathSounds.Length)]);
    }

    public void PlayPlayerShootSound()
    {
        audioSource.PlayOneShot(playerShootSounds[Random.Range(0, playerShootSounds.Length)]);
    }

    public void PlayGearCollectSound()
    {
        audioSource.PlayOneShot(gearCollectSounds[Random.Range(0, gearCollectSounds.Length)]);
    }

    public void PlaySwordCollideSound()
    {
        audioSource.PlayOneShot(swordCollideSounds[Random.Range(0, swordCollideSounds.Length)]);
    }

    public void PlaySwordSwingSound()
    {
        audioSource.PlayOneShot(swordSwingSounds[Random.Range(0, swordSwingSounds.Length)]);
    }

    public void PlaySwordLootSound()
    {
        audioSource.PlayOneShot(swordLootSounds[Random.Range(0, swordLootSounds.Length)]);
    }

    public void PlayMusic()
    {
        audioSource.clip = music[Random.Range(0, music.Length)];
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        audioSource.UnPause();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
