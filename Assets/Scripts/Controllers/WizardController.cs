using UnityEngine;

public class WizardController : MonoBehaviour
{
    public string dialogue;
    public DialogueManager dialogueManager;
    private bool isPlayerNearby;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            dialogueManager.OpenDialogueBox(dialogue);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            dialogueManager.CloseDialogueBox();
        }
    }
}