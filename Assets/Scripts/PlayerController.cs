using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player identification
    int player = 0;

    // Grounded check
    bool grounded;
    [SerializeField] LayerMask groundMask;
    const float groundCheckWidth = 0.015f;

    // Movement variables
    [Header("Horizontal movement")]
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] [Range(0,1)] float airControl;
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
    Arma arma;

    // Debug purpose
    [SerializeField] bool debug;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        arma = GetComponent<Arma>();
        InitializeVariables();
        SetPlayer();
    }

    private void SetPlayer()
    {
        if (gameObject.CompareTag("Player1"))
        {
            player = 1;
        }
        else if (gameObject.CompareTag("Player2"))
        {
            player = 2;
        }
    }

    private void InitializeVariables()
    {
        gravity = -2 * jumpHeight / (timeToApex * timeToApex);
        rb2D.gravityScale = gravity / Physics2D.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        ManageInputs();

        if (debug)
        {
            InitializeVariables();
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
                float velocityChange = acceleration * Time.deltaTime;
                if (!grounded) velocityChange *= airControl;
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
                float velocityChange = acceleration * Time.deltaTime;
                if (!grounded) velocityChange *= airControl;
                aux -= new Vector2(velocityChange, 0);
                if (aux.x < -velocityLimit) aux.x = -velocityLimit;
                rb2D.velocity = aux;
            }

        }
        else
        {
            Vector2 aux = rb2D.velocity;
            float velocityChange = deceleration * Time.deltaTime;
            if (!grounded) velocityChange *= airControl;

            if (rb2D.velocity.x > 0)
            {
                aux -= new Vector2(velocityChange, 0);
                if (aux.x < 0) aux.x = 0;
            }
            else if (rb2D.velocity.x < 0)
            {
                aux += new Vector2(velocityChange, 0);
                if (aux.x > 0) aux.x = 0;
            }
            
            rb2D.velocity = aux;
        }
    }

    private void ManageInputs()
    {
        if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                arma.Atirar();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                jumpKey = true;
            }

            horizontalMove = 0;
            if (Input.GetKey(KeyCode.D))
            {
                horizontalMove = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                horizontalMove = -1;
            }
        }
        else if (player == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jumpKey = true;
            }

            horizontalMove = 0;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalMove = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalMove = -1;
            }
        }

        /*
        if (Input.GetButtonDown("Jump"))
        {
            jumpKey = true;
        }

        float hMoveRaw = Input.GetAxisRaw("Horizontal");
        horizontalMove = (hMoveRaw > 0.15f) ? 1 : (hMoveRaw < -0.15f) ? -1 : 0;
        */
    }

    private void Jump()
    {
        rb2D.velocity += new Vector2(0, -gravity * timeToApex);
    }

    private void CheckGrounded()
    {
        Vector2 pointA, pointB;

        pointA = new Vector2(transform.position.x - transform.localScale.x / 2 + groundCheckWidth, 
            transform.position.y - transform.localScale.y / 2 + groundCheckWidth);
        pointB = new Vector2(transform.position.x + transform.localScale.x / 2 - groundCheckWidth, 
            transform.position.y - transform.localScale.y / 2 - groundCheckWidth);

        if (Physics2D.OverlapArea(pointA, pointB, groundMask) && rb2D.velocity.y <= 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
