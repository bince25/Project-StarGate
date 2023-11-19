// DialogueManager.cs
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TypewriterEffect typewriterEffect;

    void Start()
    {
        CloseDialogueBox();
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
