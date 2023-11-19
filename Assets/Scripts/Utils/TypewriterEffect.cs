// TypewriterEffect.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float charactersPerSecond = 10f;
    public bool isTyping { get; private set; }

    public TMP_Text textComponent;
    private string fullText;
    private int currentCharacterIndex;

    void Start()
    {

    }

    public void SetText(string text)
    {
        fullText = text;
        textComponent.text = string.Empty;
        StartCoroutine(TypeText());
    }

    public void ResetText()
    {
        StopAllCoroutines();
        textComponent.text = string.Empty;
        isTyping = false;
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        currentCharacterIndex = 0;

        while (currentCharacterIndex < fullText.Length)
        {
            textComponent.text += fullText[currentCharacterIndex];
            currentCharacterIndex++;

            float delay = 1f / charactersPerSecond;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }
}
