using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    [SerializeField] float speed;
    Animator animator;
    Rigidbody2D rb;
    Vector2 velocity;

    bool movementBlocked = false;

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
    
    public void StopMovement(bool stop)
    {
        if (stop)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            movementBlocked = true;
        }
        else
        {
            movementBlocked = false;
        }       
    }

    void Move()
    {
        if (movementBlocked)
            return;

        velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
        rb.velocity = velocity;

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
