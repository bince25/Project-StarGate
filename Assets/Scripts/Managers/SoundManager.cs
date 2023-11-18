using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer audioMixer; // Attach your AudioMixer here
    public AudioMixerGroup musicGroup; // Attach your Music AudioMixerGroup here
    public AudioMixerGroup soundEffectGroup;
    public AudioMixerGroup uiGroup;

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
            source.outputAudioMixerGroup = soundEffectGroup; // Assign to sound effect group by default
            audioSources.Add(source);
        }
    }

    private void PlaySound(AudioClip[] clips, AudioMixerGroup group = null)
    {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.PlayOneShot(clip);
        }

        if (group != null)
        {
            source.outputAudioMixerGroup = group;
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

    public void PlayEnemyHitSound() { PlaySound(enemyHitSounds, soundEffectGroup); }
    public void PlayEnemyDeathSound() { PlaySound(enemyDeathSounds, soundEffectGroup); }
    public void PlayPlayerHitSound() { PlaySound(playerHitSounds, soundEffectGroup); }
    public void PlayPlayerDeathSound() { PlaySound(playerDeathSounds, soundEffectGroup); }
    public void PlayPlayerShootSound() { PlaySound(playerShootSounds, soundEffectGroup); }
    public void PlayGearCollectSound() { PlaySound(gearCollectSounds, soundEffectGroup); }
    public void PlaySwordCollideSound() { PlaySound(swordCollideSounds, soundEffectGroup); }
    public void PlaySwordSwingSound() { PlaySound(swordSwingSounds, soundEffectGroup); }
    public void PlaySwordLootSound() { PlaySound(swordLootSounds, soundEffectGroup); }
    public void PlaySwordObstacleSound() { PlaySound(swordObstacleSounds, soundEffectGroup); }
    public void PlaySwordBulletSound() { PlaySound(swordBulletSounds, soundEffectGroup); }

    public void PlayMenuSound() { PlaySound(menuSounds, uiGroup); }

    // Music control methods
    public void PlayMusic()
    {
        AudioSource musicSource = GetAvailableAudioSource();
        if (music.Length > 0 && musicSource != null)
        {
            musicSource.clip = music[Random.Range(0, music.Length)];
            musicSource.loop = true; // Usually, music tracks are looped
            musicSource.outputAudioMixerGroup = musicGroup;
            musicSource.Play();
        }
    }

    public AudioSource PlayMusic(AudioClip music)
    {
        AudioSource musicSource = GetAvailableAudioSource();

        musicSource.clip = music;
        musicSource.loop = true; // Usually, music tracks are looped
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.Play();
        return musicSource;
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

    public void SetMasterVolume(float volume)
    {
        float volumeInDB;

        if (volume == 0)
        {
            // If the volume is 0, set it to -80 dB (silence).
            volumeInDB = -80;
        }
        else
        {
            // Convert the linear range (0 to 5) to a logarithmic dB scale (-80 to 0).
            // The division by 5 normalizes the slider value to a 0-1 range.
            volumeInDB = Mathf.Log10(volume / 5) * 20;
        }

        // Set the volume on the mixer. Clamp it just to be safe.
        volumeInDB = Mathf.Max(volumeInDB, -80);
        audioMixer.SetFloat("MasterVolume", volumeInDB);
    }


    public void SetPitch(float pitch)
    {
        foreach (var source in audioSources)
        {
            source.pitch = pitch;
        }
    }
}
