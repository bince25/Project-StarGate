using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    protected Animator legsAnimator;
    protected Rigidbody2D rb;
    public GameObject playersSword;

    public float moveSpeed = 10f;
    public bool isDead, godMode, killedByPlayer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Sword"))
            {
                playersSword = child.gameObject;
            }
        }
        killedByPlayer = false;
    }

    protected virtual void Start()
    {
        // Get the Animator component from the "Legs" child object
        legsAnimator = transform.Find("Sprites").transform.GetChild(transform.Find("Sprites").transform.childCount - 1).GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void FixedUpdate()
    {
        // Apply movement in FixedUpdate for physics consistency
        Vector2 moveInput = new Vector2(Input.GetAxis(GetHorizontalInput()), Input.GetAxis(GetVerticalInput()));
        Vector2 moveVelocity = moveInput.normalized * moveSpeed;
        rb.velocity = moveVelocity;
    }

    protected virtual void Move()
    {
        Vector2 movement = new Vector2(Input.GetAxis(GetHorizontalInput()), Input.GetAxis(GetVerticalInput())).normalized;

        legsAnimator.SetBool("IsWalking", movement.magnitude >= 0.1f);
        legsAnimator.SetFloat("Speed", movement.magnitude);

        UpdateSpriteFlip(movement.x);
    }
    protected virtual void UpdateSpriteFlip(float horizontal)
    {
        // Flip the sprite based on the direction of movement
        foreach (Transform child in transform.Find("Sprites").transform)
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


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (other.CompareTag("Gear"))
        {
            other.gameObject.GetComponent<GearController>().GetCollected();
        }
        else if (other.CompareTag("Sword") && (!other.transform.parent.gameObject.CompareTag("Player") || other.transform.parent.gameObject.name != transform.gameObject.name))
        {
            if (boss != null && other.transform.parent.gameObject.CompareTag("Player")) return;
            killedByPlayer = true;
            Die();
        }
        else if (other.CompareTag("Bullet"))
        {
            Die();
        }
    }

    protected virtual void Die()
    {

        if (godMode)
        {
            return;
        }
        if (isDead)
        {
            return;
        }
        isDead = true;
        SoundManager.Instance.PlayPlayerDeathSound();
        gameObject.SetActive(false);
    }



    protected virtual string GetHorizontalInput()
    {
        return "Horizontal";
    }

    protected virtual string GetVerticalInput()
    {
        return "Vertical";
    }
}
