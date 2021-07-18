using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    public Rigidbody2D rb;

    public float movementSpeed = 5;
    private Vector2 movement;

    public LayerMask interactableCheckLayer;

    public float jumpSpeed;
    public GameObject groundCheckPoint;
    public float groundCheckRadius;
    public bool hasLanded = false;
    public bool isTouchingGround = false;
    public AudioSource jumpSFX;

    public GameObject leftWallCheck;
    public GameObject rightWallCheck;
    public float wallCheckRadius;
    public float clingSpeed;
    public bool isWallTouchingLeft = false;
    public bool isWallTouchingRight = false;
    public bool isClinging = false;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);

        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, interactableCheckLayer);

        if (isTouchingGround)
        {
            isClinging = false;

            if (!hasLanded)
            {
                hasLanded = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                hasLanded = true;
                OnJump();
            }
        }

        isWallTouchingLeft = Physics2D.OverlapCircle(leftWallCheck.transform.position, groundCheckRadius, interactableCheckLayer);
        isWallTouchingRight = Physics2D.OverlapCircle(rightWallCheck.transform.position, groundCheckRadius, interactableCheckLayer);

        if (isWallTouchingLeft && !isTouchingGround && movement.x < 0)
        {
            Cling();
        }
        else if (isWallTouchingRight && !isTouchingGround && movement.x > 0)
        {
            Cling();
        }
        
        if (isClinging)
        {
            if (Input.GetButtonDown("Jump"))
            {
                WallJump(isWallTouchingLeft);
            }
        }
    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
    }

    private void Cling()
    {
        rb.velocity = new Vector2(0, -clingSpeed);
        isClinging = true;
    }

    void OnJump()
    {
        jumpSFX.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    private void WallJump(bool isLeft)
    {
        jumpSFX.Play();
        if (isLeft)
        {
            rb.velocity = new Vector2(movementSpeed, jumpSpeed);
            isClinging = false;
            hasLanded = false;
        }
        else
        {
            rb.velocity = new Vector2(-movementSpeed, jumpSpeed);
            isClinging = false;
            hasLanded = false;
        }
    }

}
