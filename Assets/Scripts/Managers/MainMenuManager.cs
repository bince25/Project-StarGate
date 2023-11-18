using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [SerializeField]
    private GameObject creditPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple MainMenuManager instances in Scene!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCredits()
    {
        creditPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditPanel.SetActive(false);
    }

    protected void PlayGame()
    {
        LoadLevel(Levels.Level1);
    }

    public void LoadLevel(Levels level)
    {
        switch (level)
        {
            case Levels.MainMenu:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case Levels.Level1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
                break;
            case Levels.Level2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
                break;
            default:
                break;
        }
    }
}
