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

    //Damage variables
    float dano = 0;
    const float danoMax = 2;
    float multiplier = 1;
    public int municao = 10;
    float shootCounter = 0;
    [SerializeField] float fireRate = 0.5f;

    [Header("Reference components")]
    [SerializeField] GameObject cratePrefab;
    GameObject ammo;
    [SerializeField] Transform spawnPosition;

    // Cached components
    BoxCollider2D bc2D;
    Rigidbody2D rb2D;
    Arma arma;

    //
    GameObject currentPlatform;

    // 
    public ParticleSystem poeira;
    public BarraKnockBack barraKB;
    public AmmoUI textoAmmo;

    // Sound variables
    [Header("Sound effects")]
    [SerializeField] float walkEffectInterval = .3f;
    float walkEffectCounter;

    // Debug purpose
    [SerializeField] bool debug;

    private void Awake()
    {
        bc2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        arma = GetComponent<Arma>();
        InitializeVariables();
        SetPlayer();
        StartCoroutine("AmmoSpawn");
    }

    private void Start()
    {
        textoAmmo.ChangeText(municao);
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
        walkEffectCounter = walkEffectInterval;
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

        // enquanto voc� anda, o efeito sonoro � tocado uma vez a cada intervalo de tempo
        if (grounded && rb2D.velocity.x != 0)
        {
            walkEffectCounter += Time.deltaTime;
            if (walkEffectCounter >= walkEffectInterval)
            {
                SoundManager.instance.PlaySoundEffects("walk");
                walkEffectCounter = 0f;
            }
        }
        else
        {
            walkEffectCounter = walkEffectInterval;
        }
    }

    private void ManageInputs()
    {
        if (player == 1)
        {
            shootCounter -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(municao > 0)
                {
                    if (shootCounter < 0)
                    {
                        if (arma.Atirar())
                        {
                            municao--;
                            SoundManager.instance.PlaySoundEffects("shoot");
                            textoAmmo.ChangeText(municao);
                        }
                        shootCounter = fireRate;
                    }

                }
                
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
                    GerarPoeira();
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
            shootCounter -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                if(municao > 0)
                {
                    if (shootCounter < 0)
                    {
                        if (arma.Atirar())
                        {
                            municao--;
                            SoundManager.instance.PlaySoundEffects("shoot");
                            textoAmmo.ChangeText(municao);
                        }
                        shootCounter = fireRate;
                    }

                }
                
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
                    GerarPoeira();
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
        GerarPoeira();
    }

    private void Jump()
    {
        rb2D.velocity += new Vector2(0, -gravity * timeToApex);
        GerarPoeira();

        //sound effect
        SoundManager.instance.PlaySoundEffects("jump");
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

        barraKB.SetValue(dano);
        // sound effect
        SoundManager.instance.PlaySoundEffects("hit");
    }

    private void CheckGrounded()
    {
        Vector2 pointA, pointB;

        pointA = new Vector2(bc2D.bounds.center.x - bc2D.bounds.extents.x + groundCheckWidth, 
            bc2D.bounds.center.y - bc2D.bounds.extents.y + groundCheckWidth);
        pointB = new Vector2(bc2D.bounds.center.x + bc2D.bounds.extents.x - groundCheckWidth, 
            bc2D.bounds.center.y - bc2D.bounds.extents.y - groundCheckWidth);

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

        pointA = new Vector2(bc2D.bounds.center.x - bc2D.bounds.extents.x + groundCheckWidth, 
            bc2D.bounds.center.y - bc2D.bounds.extents.y + groundCheckWidth);
        pointB = new Vector2(bc2D.bounds.center.x + bc2D.bounds.extents.x - groundCheckWidth, 
            bc2D.bounds.center.y - bc2D.bounds.extents.y - groundCheckWidth);

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

        pointA = bc2D.bounds.center + bc2D.bounds.extents;
        pointB = bc2D.bounds.center - bc2D.bounds.extents;

        Debug.DrawLine(pointA, pointB, Color.red);

        if (Physics2D.OverlapArea(pointA, pointB, groundMask) == null)
        {
            currentPlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
            currentPlatform = null;
        }
    }

    private void GerarPoeira()
    {
        poeira.Play();
    }

    public void Reload()
    {
        if(municao > 10){
            municao = 15;
        }
        else{
            municao += 5;
        }

        SoundManager.instance.PlaySoundEffects("ammo");
        textoAmmo.ChangeText(municao);
    }

    IEnumerator AmmoSpawn()
    {
        while (true)
        {
            ammo = Instantiate(cratePrefab, spawnPosition) as GameObject;
            ammo.transform.SetParent(null);
            yield return new WaitForSeconds(25f);
        }
        
    }

}
