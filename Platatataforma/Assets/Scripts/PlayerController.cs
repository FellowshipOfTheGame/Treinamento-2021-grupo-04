using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Grounded check
    bool grounded;
    [SerializeField] LayerMask groundMask;
    const float groundCheckWidth = 0.015f;

    // Movement variables
    [Header("Horizontal movement")]
    [SerializeField] float acelleration;
    [SerializeField] [Range(0,1)] float airDrag;
    [SerializeField] float velocityLimit;

    [Header("Jump variables")]
    [SerializeField] float jumpHeight;
    [SerializeField] float timeToApex;

    float gravity;

    // Input variables
    bool jumpKey = false;
    int horizontalMove = 0;

    // Cached components
    Rigidbody2D rb2D;

    // Debug purpose
    [SerializeField] bool debug;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        gravity = -2 * jumpHeight / (timeToApex * timeToApex);
        Physics2D.gravity = new Vector2(0, gravity);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (debug)
        {
            // recalculate variables
            gravity = -2 * jumpHeight / (timeToApex * timeToApex);
            Physics2D.gravity = new Vector2(0, gravity);
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (jumpKey)
        {
            jumpKey = false;
            if (grounded)
                Jump();
        }

        if (horizontalMove == 1)
        {
            Vector2 aux = rb2D.velocity;
            if (aux.x < velocityLimit)
            {
                float velocityChange = acelleration * Time.deltaTime;
                if (!grounded) velocityChange *= 1 - airDrag;
                aux += new Vector2(velocityChange, 0);
                if (aux.x > velocityLimit) aux.x = velocityLimit;
                rb2D.velocity = aux;
            }

        }
        else if (horizontalMove == -1)
        {
            Vector2 aux = rb2D.velocity;
            if (aux.x > -velocityLimit)
            {
                float velocityChange = acelleration * Time.deltaTime;
                if (!grounded) velocityChange *= 1 - airDrag;
                aux -= new Vector2(velocityChange, 0);
                if (aux.x < -velocityLimit) aux.x = -velocityLimit;
                rb2D.velocity = aux;
            }

        }
        else
        {
            Vector2 aux = rb2D.velocity;
            float velocityChange = acelleration * Time.deltaTime;
            if (!grounded) velocityChange *= 1 - airDrag;

            if (rb2D.velocity.x > 0)
            {
                aux -= new Vector2(velocityChange, 0);
                if (aux.x < 0) aux.x = 0;
                rb2D.velocity = aux;
            }
            else if (rb2D.velocity.x < 0)
            {
                aux += new Vector2(velocityChange, 0);
                if (aux.x > 0) aux.x = 0;
                rb2D.velocity = aux;
            }
        }
    }

    private void GetInputs()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpKey = true;
        }

        float hMoveRaw = Input.GetAxisRaw("Horizontal");
        horizontalMove = (hMoveRaw > 0.15f) ? 1 : (hMoveRaw < -0.15f) ? -1 : 0;
    }

    private void Jump()
    {
        rb2D.AddForce(transform.up * -gravity * timeToApex, ForceMode2D.Impulse);

        //rb2D.AddForce(new Vector2(1, 1).normalized * -gravity * timeToApex, ForceMode2D.Impulse);
    }

    private void CheckGrounded()
    {
        Vector2 pointA, pointB;

        pointA = new Vector2(transform.position.x - transform.localScale.x / 2 + groundCheckWidth, 
            transform.position.y - transform.localScale.y / 2 + groundCheckWidth);
        pointB = new Vector2(transform.position.x + transform.localScale.x / 2 - groundCheckWidth, 
            transform.position.y - transform.localScale.y / 2 - groundCheckWidth);

        if (Physics2D.OverlapArea(pointA, pointB, groundMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
