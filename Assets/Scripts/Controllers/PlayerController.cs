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
    private Rigidbody2D rb;
    private GameObject playersSword;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Start()
    {
        // Get the Animator component from the "Legs" child object
        legsAnimator = spritesObject.transform.GetChild(spritesObject.transform.childCount - 1).GetComponent<Animator>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Sword"))
            {
                playersSword = child.gameObject;
                Debug.Log("Found sword!: ", playersSword);
            }
        }
    }
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector2 movement = new Vector2(horizontal, vertical).normalized;

        // Set parameters for Animator based on movement
        legsAnimator.SetBool("IsWalking", movement.magnitude >= 0.1f);
        legsAnimator.SetFloat("Speed", movement.magnitude);

        UpdateSpriteFlip(horizontal);
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics consistency
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 moveVelocity = moveInput.normalized * moveSpeed;
        rb.velocity = moveVelocity;
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
    public void SwitchSwordPrefab(GameObject newSwordPrefab)
    {
        // Destroy the current sword prefab
        Destroy(playersSword);

        // Instantiate the new sword prefab as a child of the player
        Vector3 swordPosition = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        GameObject newSword = Instantiate(newSwordPrefab, swordPosition, Quaternion.identity);
        newSword.transform.parent = transform;

        // Update the currentSwordPrefab reference
        playersSword = newSwordPrefab;
    }
}
