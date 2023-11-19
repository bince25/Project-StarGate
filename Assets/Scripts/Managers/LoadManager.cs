using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance { get; private set; }

    public Slider progressBar;
    public TMP_Text progressText;
    public Image background;
    public AudioClip bgMusic;
    private AudioSource audioSource;

    private Levels loadingSceneName = Levels.LoadingScene; // Replace with your actual loading scene name

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    public void LoadScene(Levels sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }


    public void LoadSceneWithTransition(Levels sceneName)
    {
        audioSource = SoundManager.Instance.PlayMusic(bgMusic, 0.5f);
        SceneManager.LoadScene((int)loadingSceneName);
        // First, load the loading scene

        // Then start loading the target scene asynchronously
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(Levels sceneName)
    {
        yield return new WaitForSeconds(1.5f);

        SoundManager.Instance.StopMusic(audioSource, 0.2f);

        progressBar = LoadScreenUIManager.Instance.progressBar;
        progressText = LoadScreenUIManager.Instance.progressText;

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null) progressBar.value = progress;
            if (progressText != null) progressText.text = (int)(progress * 100f) + "%";

            yield return null;
        }
    }

    public void SetBackground(Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;
    }
}
