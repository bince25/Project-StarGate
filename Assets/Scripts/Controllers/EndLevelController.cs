using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public AudioClip levelMusic;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = SoundManager.Instance.PlayMusic(levelMusic, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.StopMusic(audioSource, 0.5f);
            StartCoroutine(EndLevelCoroutine());
        }
    }

    public void EndLevel()
    {
        if (ResourceManager.Instance.gearCount > 0)
        {
            LoadManager.Instance.LoadSceneWithTransition((Levels)(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(.7f);
        LoadManager.Instance.LoadSceneWithTransition((Levels)(SceneManager.GetActiveScene().buildIndex + 1));
    }
}