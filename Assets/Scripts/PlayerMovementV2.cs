using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    public Rigidbody2D rb;

    public float movementSpeed = 5;
    private Vector2 movement;

    public float jumpSpeed;
    public GameObject groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    public bool hasLanded = false;
    public bool isTouchingGround = false;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);

        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, groundCheckLayer);

        if (isTouchingGround && !hasLanded)
        {
            hasLanded = false;
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            hasLanded = true;
            OnJump();
        }
    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
    }

    void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

}
