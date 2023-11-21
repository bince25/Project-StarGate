using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Player
{
    protected override void Awake()
    {
        base.Awake();
        // Add additional initialization for Player 1 if needed
    }

    protected override void Start()
    {
        base.Start();
        // Add additional initialization for Player 1 if needed
    }

    protected override void Update()
    {
        base.Update();
        // Add additional update logic for Player 1 if needed
    }

    protected override void Move()
    {
        base.Move();
        // Add additional movement logic for Player 1 if needed
    }

    protected override void UpdateSpriteFlip(float horizontal)
    {
        // Add custom sprite flipping logic for Player 1 if needed
        base.UpdateSpriteFlip(horizontal);
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Add additional collision handling logic for Player 1 if needed
    }

    public override void Die()
    {
        base.Die();
        // Add additional death logic for Player 1 if needed
    }

    protected override string GetHorizontalInput()
    {
        return "Horizontal";
    }

    protected override string GetVerticalInput()
    {
        return "Vertical";
    }

    protected override void Dash()
    {
        if (Time.time > lastDashTime + dashCooldown && Input.GetKey(KeyCode.LeftShift))
        {
            SoundManager.Instance.PlayDashSound();
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
}
