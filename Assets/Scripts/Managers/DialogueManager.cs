// DialogueManager.cs
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialogueBox;
    public TypewriterEffect typewriterEffect;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {

    }

    public void OpenDialogueBox(string dialogue)
    {
        dialogueBox.SetActive(true);
        typewriterEffect.SetText(dialogue);
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        typewriterEffect.ResetText();
    }
}
