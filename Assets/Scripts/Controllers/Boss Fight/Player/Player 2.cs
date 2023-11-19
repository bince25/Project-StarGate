using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Player
{
    protected override void Awake()
    {
        base.Awake();
        // Add additional initialization for Player 2 if needed
    }

    protected override void Start()
    {
        base.Start();
        // Add additional initialization for Player 2 if needed
    }

    protected override void Update()
    {
        base.Update();
        // Add additional update logic for Player 2 if needed
    }

    protected override void Move()
    {
        base.Move();
        // Add additional movement logic for Player 2 if needed
    }

    protected override void UpdateSpriteFlip(float horizontal)
    {
        // Add custom sprite flipping logic for Player 2 if needed
        base.UpdateSpriteFlip(horizontal);
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        // Add additional collision handling logic for Player 2 if needed
    }

    protected override void Die()
    {
        base.Die();
        // Add additional death logic for Player 2 if needed
    }

    protected override string GetHorizontalInput()
    {
        return "Horizontal2";
    }

    protected override string GetVerticalInput()
    {
        return "Vertical2";
    }
}