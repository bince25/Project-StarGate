using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadScreenUIManager : MonoBehaviour
{
    public static LoadScreenUIManager Instance { get; private set; }

    public Slider progressBar;
    public TMP_Text progressText;
    public Image background;

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
}