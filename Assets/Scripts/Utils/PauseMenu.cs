using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseOptions;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseOptions.SetActive(false);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseOptions.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseOptions.SetActive(false);
        LoadManager.Instance.LoadSceneWithTransition(Levels.MainMenu);
    }
}
