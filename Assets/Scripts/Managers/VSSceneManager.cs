using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VSSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Arena", 1);
        DialogueManager.Instance.OpenDialogueBox("Now Only One Of You Will Survive !!!");
        StartCoroutine(CloseDialogue());
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length <= 1)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(3);
        DialogueManager.Instance.CloseDialogueBox();
    }
}
