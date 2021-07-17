using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0f;
    public float accel = 0f;

    public float inputDeadZone = 0.1f;
    public float inputAngSnapZone = 5f;
    public float maxSpeed = 1.2f;
    public float maxForce = 10f;
    public float startForce = 0.5f;
    public float curvePow = 0.5f;
    public float speedDecay = 5f;
    public float stopSpeed = 0.1f;

    public float recoilForce = 30f;

    public Vector2 inputVec;
    public Vector2 netForce;

    public bool controlEnabled = true;
    public bool fire1Held = false;
    public int recoilTime = 3;
    public int recoilFrameCounter = 0;

    public Vector2[] inputAngSnap;
    public Vector2 playerToMouseDir;
    public Vector2 mousePosition;

    public Rigidbody2D rb;

    public float jumpSpeed;
    public GameObject groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    private bool hasLanded = false;
    private bool isTouchingGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        recoilFrameCounter = recoilTime;
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    // called at set intervals (physics stuff here)
    void FixedUpdate()
    {
        // force vector to be applied at the end of this update
        netForce = new Vector2(0, 0);
        
        if (controlEnabled) {

            inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            netForce += inputToMovement(inputVec);

            if (recoilFrameCounter < recoilTime) {
                netForce += recoilVel(playerToMouseDir);
                recoilFrameCounter++;
            }
            else if (Input.GetButton("Fire1")) {
                if (!fire1Held) {
                    mousePosition = Input.mousePosition;
                    mousePosition.x -= (Screen.width / 2);
                    mousePosition.y -= (Screen.height / 2);
                    playerToMouseDir = mousePosition - rb.position;
                    playerToMouseDir.Normalize();
                    netForce += recoilVel(playerToMouseDir);
                    fire1Held = true;
                    recoilFrameCounter = 0;
                }
            }
            else {
                fire1Held = false;
            }
        }

        // apply net force to player
        netForce.y = 0;
        rb.AddForce(netForce);
    }

    // assuming input * maxSpeed is the inteded velocity by the player and outputs solution aimed at intended velocity
    // outputs a force vector to be applied on the player rigidbody
    // inputs a vector of horizontal and vertical input values
    Vector2 inputToMovement (Vector2 input)
    {
        Vector2 output = new Vector2(0, 0);
        // formatting vector2 input
        if (input.magnitude > inputDeadZone) {
            // snapping input to 1 magnitude or capping it at 1 magnitude
            if (1 - input.magnitude < inputDeadZone)
            {
                input.Normalize();
            }
            // snapping input to angles
            foreach (Vector2 i in inputAngSnap) {
                if (Vector2.Angle(i, input) < inputAngSnapZone) {
                    input = i * input.magnitude;
                }
            }
        }
        else {
            input = new Vector2(0, 0);
        }

        // difference in velocity between target velocity and current velocity
        Vector2 deltaVel = (input * maxSpeed) - rb.velocity;
        // force = mass * accel = mass * speed / time
        Vector2 deltaForce = deltaVel * rb.mass / Time.deltaTime;
        // capping deltaForce to a function relative to speed
        // the funciton is a root function with an offset
        // offset of startForce is necessary for the player to start moving from standstill
        // the smaller the curvePow, the more aggressive the curve is
        float forceCap = (maxForce - startForce) * (float)Math.Pow(rb.velocity.magnitude / maxSpeed, curvePow) + startForce;
        if (deltaForce.magnitude > forceCap) {
            deltaForce = deltaForce.normalized * forceCap;
        }
        return deltaForce;
    }

    /*
        Vector2 inputToMovement2 (Vector2 input)
    {
        Vector2 output = new Vector2(0, 0);
        // formatting vector2 input
        if (input.magnitude > inputDeadZone && rb.velocity.magnitude < maxSpeed) {
            // snapping input to 1 magnitude or capping it at 1 magnitude
            if (1 - input.magnitude < inputDeadZone)
            {
                input.Normalize();
            }
            // snapping input to angles
            foreach (Vector2 i in inputAngSnap) {
                if (Vector2.Angle(i, input) < inputAngSnapZone) {
                    input = i * input.magnitude;
                }
            }
            output = input * maxForce;
            // check if output would put speed above maximum
            Vector2 deltaVel = output * Time.deltaTime / rb.mass; // the speed change that would happen if output was applied
            Vector2 potentialVel = rb.velocity + deltaVel; // the resulting speed after applying the speed change
            if (potentialVel.magnitude > maxSpeed) {
                // desiredDeltaVel is rotated as if uncapped but is then capped at maxSpeed
                Vector2 desiredDeltaVel = potentialVel.normalized * maxSpeed - rb.velocity;
                output = rb.mass * desiredDeltaVel / Time.deltaTime;
            }
        }
        else {
            input = new Vector2(0,0);
            if (rb.velocity.magnitude > stopSpeed) {
                // apply speedDecay
                output = rb.velocity.normalized * -1 * speedDecay;
            }
            else {
                // set speed to 0 
                output = rb.mass * rb.velocity * -1 / Time.deltaTime;
            }
        }

        return output;
    }
    */

    Vector2 recoilVel(Vector2 direction)
    {
        return direction.normalized * -1 * recoilForce;
    }
}