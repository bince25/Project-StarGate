using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void PlayGame()
    {
        LoadLevel(Levels.Level1);
    }

    public void LoadLevel(Levels level)
    {
        Debug.Log("Loading level " + (int)level);
        LoadManager.Instance.LoadSceneWithTransition(level);
    }
}
