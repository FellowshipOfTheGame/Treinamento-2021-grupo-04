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
    const float groundCheckWidth = 0.015f; // este valor foi testado

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

    // Sprite related
    bool facingRight = true;

    // Input variables
    bool jumpKey = false;
    int horizontalMove = 0;

    //Damage variable
    float dano = 0;
    const float danoMax = 2;
    float multiplier = 1;

    // Cached components
    Rigidbody2D rb2D;
    Arma arma;

    //
    GameObject currentPlatform;

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

    public int GetPlayer()
    {
        return player;
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
        if (currentPlatform != null)
        {
            ManagePlatform();
        }

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

            if (!facingRight) FlipPlayer();
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

            if (facingRight) FlipPlayer();
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                arma.Atirar();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                jumpKey = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (currentPlatform == null)
                {
                    currentPlatform = FindPlaform();
                }
                else
                {
                    currentPlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
                }
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
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                arma.Atirar();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jumpKey = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {   
                if (currentPlatform == null)
                {
                    currentPlatform = FindPlaform();
                }
                else
                {
                    currentPlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
                }
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
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
            
        if (facingRight)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    private void Jump()
    {
        rb2D.velocity += new Vector2(0, -gravity * timeToApex);
    }

    public void Knockback(Vector2 force, float danoProjetil)
    {
        if(dano < danoMax){
            if(dano + danoProjetil < danoMax) dano += danoProjetil;
            else dano = danoMax;
        }
        multiplier = dano + 1;

        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(force * multiplier, ForceMode2D.Impulse);
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

    private GameObject FindPlaform()
    {
        Vector2 pointA, pointB;

        pointA = new Vector2(transform.position.x - transform.localScale.x / 2 + groundCheckWidth,
            transform.position.y - transform.localScale.y / 2 + groundCheckWidth);
        pointB = new Vector2(transform.position.x + transform.localScale.x / 2 - groundCheckWidth,
            transform.position.y - transform.localScale.y / 2 - groundCheckWidth);

        if (Physics2D.OverlapArea(pointA, pointB, groundMask))
        {
            return Physics2D.OverlapArea(pointA, pointB, groundMask).gameObject;
        }
        else
        {
            return null;
        }
    }

    private void ManagePlatform()
    {
        Vector2 pointA, pointB;

        pointA = transform.position + transform.localScale * 0.5f;
        pointB = transform.position - transform.localScale * 0.5f;

        Debug.DrawLine(pointA, pointB, Color.red);

        if (Physics2D.OverlapArea(pointA, pointB, groundMask) == null)
        {
            currentPlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
            currentPlatform = null;
        }
    }
}
