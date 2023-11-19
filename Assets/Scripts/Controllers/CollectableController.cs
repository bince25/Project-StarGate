using UnityEngine;
using DG.Tweening;

public class CollectableController : MonoBehaviour
{
    public GameObject replacementSwordPrefab;

    void Start()
    {
        // Set the initial sprite
        GetComponent<SpriteRenderer>().sprite = replacementSwordPrefab.GetComponent<SpriteRenderer>().sprite;

        // Play the looping bounce animation
        PlayLoopingBounceAnimation();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Display a message or UI hint for the player to press 'E'
            ResourceManager.Instance.ActivationOfItemNotification(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Call a method in the PlayerController to switch the sword prefab
                other.GetComponent<PlayerController>().SwitchSwordPrefab(replacementSwordPrefab);

                // Destroy the collectible item
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the message or UI hint for the player to press 'E'
            ResourceManager.Instance.ActivationOfItemNotification(false);

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            // Call a method in the PlayerController to switch the sword prefab
            other.GetComponent<PlayerController>().SwitchSwordPrefab(replacementSwordPrefab);

            // Destroy the collectible item
            Destroy(gameObject);
        }
    }

    void PlayLoopingBounceAnimation()
    {
        // Use DoTween to create a looping bounce animation
        Sequence bounceSequence = DOTween.Sequence();

        bounceSequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 0.5f)
            .SetEase(Ease.OutQuad));

        bounceSequence.Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f)
            .SetEase(Ease.InQuad));

        // Set the animation to loop infinitely
        bounceSequence.SetLoops(-1);
    }
}
