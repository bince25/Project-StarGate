using System.Collections;
using UnityEngine;

public class SoundFadeInOut : MonoBehaviour
{
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    public float fadeDuration = 2f;

    void Start()
    {
        // Initialize the AudioSource
        audioSource = SoundManager.Instance.PlayMusic(backgroundMusic);

        // Set up the audio settings
        audioSource.loop = true;
        audioSource.volume = 0f; // Start with zero volume

        //audioSource.Play();

        // Start playing the audio source with fade-in effect
        StartCoroutine(FadeInAudio());
    }

    IEnumerator FadeInAudio()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is exactly 1 at the end
        audioSource.volume = 1f;
    }

    IEnumerator FadeOutAudio()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is exactly 0 at the end
        audioSource.volume = 0f;
        audioSource.Stop(); // Stop the audio source after fading out
    }

    // Call this method to trigger the fade-out effect
    public void StopAudioWithFadeOut()
    {
        StartCoroutine(FadeOutAudio());
    }
}
