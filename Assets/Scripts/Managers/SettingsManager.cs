using UnityEngine;
using UnityEngine.UI; // For UI elements
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro; // For audio management

public class SettingsManager : MonoBehaviour
{
    public SettingsManager Instance { get; private set; }

    public AudioMixer audioMixer; // Attach your AudioMixer here
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public Slider soundSlider;

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
            return;
        }
    }


    void Start()
    {
        LoadSettings();
        // Resolution settings initialization
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

        soundSlider.onValueChanged.AddListener(delegate { SetSoundLevel(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
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
        // Example: Save volume level
        float volumeLevel = soundSlider.value;
        PlayerPrefs.SetFloat("VolumeLevel", volumeLevel);

        // Save other settings similarly
        // ...

        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        // Sound settings
        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            float volumeLevel = PlayerPrefs.GetFloat("VolumeLevel");
            Debug.Log("Loaded Volume Level: " + volumeLevel);
            SetVolume(volumeLevel);
            soundSlider.value = volumeLevel; // Update the UI slider
        }
        else
        {
            soundSlider.value = GameConstants.DEFAULT_VOLUME_LEVEL; // Default value
            Debug.Log("Setting Default Volume Level: " + GameConstants.DEFAULT_VOLUME_LEVEL);
            SetSoundLevel();
        }


        // Add similar logs for other settings like resolution, quality, etc.
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
