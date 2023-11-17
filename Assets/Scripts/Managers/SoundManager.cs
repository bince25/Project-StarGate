using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // Sound effect arrays
    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDeathSounds;
    public AudioClip[] playerHitSounds;
    public AudioClip[] playerDeathSounds;
    public AudioClip[] playerShootSounds;
    public AudioClip[] gearCollectSounds;
    public AudioClip[] swordCollideSounds;
    public AudioClip[] swordSwingSounds;
    public AudioClip[] swordLootSounds;
    public AudioClip[] swordObstacleSounds;
    public AudioClip[] swordBulletSounds;
    public AudioClip[] menuSounds;
    public AudioClip[] music;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private int audioSourcePoolSize = 10; // Adjust as needed

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioSources()
    {
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }
    }

    private void PlaySound(AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null; // Consider expanding the pool size if this happens
    }

    public void PlayEnemyHitSound() { PlaySound(enemyHitSounds); }
    public void PlayEnemyDeathSound() { PlaySound(enemyDeathSounds); }
    public void PlayPlayerHitSound() { PlaySound(playerHitSounds); }
    public void PlayPlayerDeathSound() { PlaySound(playerDeathSounds); }
    public void PlayPlayerShootSound() { PlaySound(playerShootSounds); }
    public void PlayGearCollectSound() { PlaySound(gearCollectSounds); }
    public void PlaySwordCollideSound() { PlaySound(swordCollideSounds); }
    public void PlaySwordSwingSound() { PlaySound(swordSwingSounds); }
    public void PlaySwordLootSound() { PlaySound(swordLootSounds); }
    public void PlaySwordObstacleSound() { PlaySound(swordObstacleSounds); }
    public void PlaySwordBulletSound() { PlaySound(swordBulletSounds); }

    // Music control methods
    public void PlayMusic()
    {
        AudioSource musicSource = GetAvailableAudioSource();
        if (music.Length > 0 && musicSource != null)
        {
            musicSource.clip = music[Random.Range(0, music.Length)];
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        foreach (var source in audioSources)
        {
            if (source.clip != null && source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void SetVolume(float volume)
    {
        foreach (var source in audioSources)
        {
            source.volume = volume;
        }
    }

    public void SetPitch(float pitch)
    {
        foreach (var source in audioSources)
        {
            source.pitch = pitch;
        }
    }
}
