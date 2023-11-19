using UnityEngine;
using UnityEngine.UI; // For UI elements
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro; // For audio management

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public AudioMixer audioMixer; // Attach your AudioMixer here
    public TMP_Dropdown resolutionDropdown;
    public Slider soundSlider;
    public Toggle fullScreenToggle;

    public Toggle soundEffectsToggle;
    public Toggle musicToggle;

    Resolution[] resolutions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeSettings();
    }

    void InitializeSettings()
    {
        LoadSettings();
        InitializeResolutionSettings();
        InitializeSoundSettings();
        InitializeFullScreenSettings();
        InitializeSoundEffectToggle();
        InitializeMusicToggle();
    }

    void InitializeSoundEffectToggle()
    {
        soundEffectsToggle.onValueChanged.AddListener(SetSoundEffects);
    }

    void InitializeMusicToggle()
    {
        musicToggle.onValueChanged.AddListener(SetMusic);
    }

    public void SetSoundEffects(bool isEnabled)
    {
        audioMixer.SetFloat("SoundEffectVolume", isEnabled ? 0 : -80); // Assuming -80 dB is silence
        PlayerPrefs.SetInt("SoundEffectsEnabled", isEnabled ? 1 : 0);
    }

    public void SetMusic(bool isEnabled)
    {
        audioMixer.SetFloat("MusicVolume", isEnabled ? 0 : -80); // Assuming -80 dB is silence
        PlayerPrefs.SetInt("MusicEnabled", isEnabled ? 1 : 0);
    }


    void InitializeResolutionSettings()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
    }

    void InitializeSoundSettings()
    {
        soundSlider.onValueChanged.AddListener(delegate { SetSoundLevel(); });
    }

    void InitializeFullScreenSettings()
    {
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        SaveSettings();
    }

    public void SetSoundLevel()
    {
        float soundLevel = soundSlider.value;
        SetVolume(soundLevel);

        // Optionally save settings immediately when changed
        SaveSettings();
    }

    public void SetVolume(float volume)
    {
        SoundManager.Instance.SetMasterVolume(volume);
        SoundManager.Instance.audioMixer.SetFloat("SoundEffectVolume", -30);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumeLevel", soundSlider.value);
        PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            float volumeLevel = PlayerPrefs.GetFloat("VolumeLevel");
            SetVolume(volumeLevel);
            soundSlider.value = volumeLevel;
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
            SetFullScreen(isFullScreen);
            fullScreenToggle.isOn = isFullScreen;
        }

        if (PlayerPrefs.HasKey("SoundEffectsEnabled"))
        {
            bool soundEffectsEnabled = PlayerPrefs.GetInt("SoundEffectsEnabled") == 1;
            soundEffectsToggle.isOn = soundEffectsEnabled;
            SetSoundEffects(soundEffectsEnabled);
        }

        if (PlayerPrefs.HasKey("MusicEnabled"))
        {
            bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled") == 1;
            musicToggle.isOn = musicEnabled;
            SetMusic(musicEnabled);
        }
    }



    public void ResetToDefaults()
    {
        int defaultResolutionIndex = resolutions.Length - 1;

        SetVolume(GameConstants.DEFAULT_VOLUME_LEVEL);
        soundSlider.value = GameConstants.DEFAULT_VOLUME_LEVEL; // Update the UI slider
        SetQuality(GameConstants.DEFAULT_QUALITY_INDEX);
        SetFullscreen(GameConstants.DEFAULT_IS_FULL_SCREEN);
        SetResolution(defaultResolutionIndex);
        resolutionDropdown.value = defaultResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SaveSettings();
    }

}
