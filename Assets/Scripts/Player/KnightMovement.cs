﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lerpSpeed = 25f;
    Animator animator;
    Rigidbody2D rb;
    Vector2 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }
    private void Update()
    {
        Move();
    }

    void Move()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed); 
        rb.velocity = Vector2.Lerp(rb.velocity, velocity, Time.deltaTime * lerpSpeed);

        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            animator?.SetBool("isWalking", true);
        }
        else
        {
            animator?.SetBool("isWalking", false);
        }
    }
}
