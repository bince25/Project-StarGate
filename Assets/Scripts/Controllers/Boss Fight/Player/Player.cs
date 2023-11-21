using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected Animator legsAnimator;
    protected Rigidbody2D rb;
    public GameObject playersSword;

    public float moveSpeed = 10f;
    public bool isDead, godMode, killedByPlayer;

    public float dashSpeed = 25f;
    public float dashDuration = 0.7f;
    public float dashCooldown = 1f;

    [SerializeField]
    private Slider slider;

    protected float lastDashTime;
    protected bool isDashing;
    protected Vector2 dashDirection;

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
        lastDashTime = Time.time;
    }

    protected virtual void Start()
    {
        // Get the Animator component from the "Legs" child object
        legsAnimator = transform.Find("Sprites").transform.GetChild(transform.Find("Sprites").transform.childCount - 1).GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        slider.value = Math.Min(Math.Min(Time.time - lastDashTime, dashCooldown) / dashCooldown, 1);
        if (!isDashing)
        {
            Move();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis(GetHorizontalInput()), Input.GetAxis(GetVerticalInput()));
            Vector2 moveVelocity = moveInput.normalized * moveSpeed;
            rb.velocity = moveVelocity;
        }

        Dash();
    }

    protected virtual void Move()
    {
        Vector2 movement = new Vector2(Input.GetAxis(GetHorizontalInput()), Input.GetAxis(GetVerticalInput())).normalized;

        //legsAnimator.SetBool("IsWalking", movement.magnitude >= 0.1f);
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

    protected virtual void Dash()
    {
        if (Time.time > lastDashTime + dashCooldown && Input.GetKey(KeyCode.LeftShift))
        {
            isDashing = true;
            lastDashTime = Time.time;

            // Store the current movement direction for dashing
            dashDirection = new Vector2(Input.GetAxis(GetHorizontalInput()), Input.GetAxis(GetVerticalInput())).normalized;
        }

        if (isDashing)
        {
            if (Time.time < lastDashTime + dashDuration)
            {
                rb.velocity = dashDirection * dashSpeed;
            }
            else
            {
                isDashing = false;
            }
        }
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
