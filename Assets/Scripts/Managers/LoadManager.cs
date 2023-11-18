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

        SceneManager.LoadScene((int)loadingSceneName);
        // First, load the loading scene

        // Then start loading the target scene asynchronously
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(Levels sceneName)
    {
        yield return new WaitForSeconds(0.5f);

        progressBar = LoadScreenUIManager.Instance.progressBar;
        progressText = LoadScreenUIManager.Instance.progressText;

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = (int)(progress * 100f) + "%";

            yield return null;
        }
    }

    public void SetBackground(Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;
    }
}
