using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private bool isDead = false;
    [SerializeField]
    private GameObject spritesObject;
    private Animator legsAnimator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Get the Animator component from the "Legs" child object
        legsAnimator = spritesObject.transform.GetChild(spritesObject.transform.childCount - 1).GetComponent<Animator>();
    }
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;



        // Check if there is any input
        if (movement.magnitude >= 0.1f)
        {
            // Calculate movement vector and move the character without changing rotation
            Vector3 moveDir = new Vector3(horizontal, vertical, 0f).normalized;
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

            // Set parameters for Animator
            legsAnimator.SetBool("IsWalking", true);
            legsAnimator.SetFloat("Speed", movement.magnitude);
        }
        else
        {
            // If there is no input, set parameters for idle
            legsAnimator.SetBool("IsWalking", false);
            legsAnimator.SetFloat("Speed", 0f);
        }
        UpdateSpriteFlip(horizontal);
    }
    void UpdateSpriteFlip(float horizontal)
    {
        // Flip the sprite based on the direction of movement
        foreach (Transform child in spritesObject.transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (horizontal < 0)
            {
                // Move left, flip the sprite
                spriteRenderer.flipX = true;
            }
            else if (horizontal > 0)
            {
                // Move right, unflip the sprite
                spriteRenderer.flipX = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gear"))
        {
            other.gameObject.GetComponent<GearController>().GetCollected();
        }
        else if (other.CompareTag("Sword") && !other.transform.parent.gameObject.CompareTag("Player"))
        {
            Die();
        }
        else if (other.CompareTag("Bullet"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        SoundManager.Instance.PlayPlayerDeathSound();
        gameObject.SetActive(false);
    }
}
