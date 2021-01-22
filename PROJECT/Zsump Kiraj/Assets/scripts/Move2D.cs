using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using System;

public class Move2D : MonoBehaviour
{
    //sétáláshoz szükséges változók

    public float walkSpeed = 5f;
    private float currSpeed = 4f;
    private float moveInput;
    Vector2 movement, movementOrder;
    public bool moveLeft = false, moveRight = false;

    // ugráshoz szükséges dolgok

    public bool canJump = true;
    public float jumpValue = 0.0f;

    //a talaj detektálásához szükséges változók

    public bool isGrounded;
    public bool isIcy;
    public bool isSticky;
    public bool is45;
    public LayerMask groundMask;
    public LayerMask icyMask;
    public LayerMask stickyMask;
    public LayerMask fourtyFiveMask;

    //segédváltozók

    public float lastPosY;
    public float currPosY;
    public float maxHeightWithoutBang = 11;


    public float XVELOCITY;


    //kötelezõ komponensek, tulajdonságok

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public PhysicsMaterial2D bounceMat, normalMat;
    public AudioClip JumpAudio;
    public AudioClip LandAudio;
    public AudioClip BangAudio;
    public AudioClip BounceAudio;
    public AudioSource audioSource;

    // A Start az elsõ képkocka frissülés elõtt hívódik meg
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Az Update képkockánként egyszer hívódik meg
    void Update()
    {


    }

    // A FixedUpdate annyiszor hívódik meg amennyit beállítunk neki az Edit >> Project Settings >> Time menüpont alatt
    // Az Update hívása kevésnek bizonyult, ezért kellett a FixedUpdate >> simább játékmenet, dinamikusabb animációk

    void FixedUpdate()
    {

        XVELOCITY = rb.velocity.x;

        AudioManager();
        BounceCheck();
        FourtyFiveCheck();
        IcyCheck();
        StickyCheck();
        GroundCheck();
        Move();
        Jump();
        StuckCharacter();

    }


    void StuckCharacter()
    {/*
        if(!isGrounded && rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(0.1f, 0);
        }*/
    }

    // ez a függvény megnézi hogy a földön van-e a karakter

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.80f, 0.05f), 0f, groundMask);
        animator.SetBool("Grounded", isGrounded);
    }

    //ez a függvény megnézi hogy jeges földön van-e a karakter

    void IcyCheck()
    {
        isIcy = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.80f, 0.05f), 0f, icyMask);
    }

    //ez a függvény megnézi, hogy ragadós földön van-e a karakter

    void StickyCheck()
    {
        isSticky = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.80f, 0.05f), 0f, stickyMask);
    }

    //ez a függvény megnézi, hogy 45-fokos földön van-e a karakter

    void FourtyFiveCheck()
    {
        bool left = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f), 0f, fourtyFiveMask);
        bool right = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f), 0f, fourtyFiveMask);

        if(right || left)
        {
            is45 = true;
            rb.sharedMaterial = normalMat;
        }
        else
        {
            is45 = false;
        }

    }

    //ez a függvény azt vizsgálja hogy az aktuális ugrás közben pattant-e már vissza falról a karakter (animáció hoz szükséges)

    void BounceCheck()
    {
        bool left = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.12f), 0f, groundMask);
        bool right = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.12f), 0f, groundMask);

        if(right || left && !isGrounded)
        {
            animator.SetBool("Bounced", true);
        }

        if (isGrounded)
        {
            animator.SetBool("Bounced", false);
        }

    }

    //ebben a függvényben van megvalósítva a horizontális mozgás

    void Move()
    {
        // sebesség beállítása, sprite renderer y tengelyen való tükrözése, hogy mindg arra nézzena karakter amerra mozogni szeretne vagy amerre utoljára mozgott

        moveInput = Input.GetAxisRaw("Horizontal") * walkSpeed;
        if (isGrounded && moveInput < 0)
        {
            animator.SetBool("Bang", false);
            sr.flipX = false;
        }
        else if (isGrounded && moveInput > 0)
        {
            animator.SetBool("Bang", false);
            sr.flipX = true;
        }

        //normal sétálás

        if (jumpValue == 0.0f && isGrounded && !isIcy && !isSticky)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            rb.velocity = new Vector2(moveInput, rb.velocity.y);
        }

        //ragadós sétálás

        if (jumpValue == 0.0f && isGrounded && !isIcy && isSticky)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", rb.velocity.x);
        }

        //jeges sétálás

        if (!isGrounded)
        {
            movement.x = rb.velocity.x / 2;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveLeft = true;
            animator.SetFloat("Speed", 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveRight = true;
            animator.SetFloat("Speed", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            moveLeft = false;
            animator.SetFloat("Speed", 0);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveRight = false;
            animator.SetFloat("Speed", 0);
        }


        if (jumpValue == 0.0f && isGrounded && isIcy && !isSticky)
        {

            if (moveLeft && !moveRight)
            {
                movementOrder.x = -walkSpeed-2;
            }
            else if (moveRight && !moveLeft)
            {
                movementOrder.x = walkSpeed+2;
            }
            else if (moveLeft && moveRight)
            {
                movementOrder.x = 0;
            }


            if(moveLeft && movement.x > movementOrder.x)
            {
                movement.x -= currSpeed * Time.fixedDeltaTime;
            }
            if(moveRight && movement.x < movementOrder.x)
            {
                movement.x += currSpeed * Time.fixedDeltaTime;
            }

            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }



    }

    //ebben a függvényben van megvalósítva a vertikális mozgás

    void Jump()
    {

        // a fall animáció kezelése a szerint hogy az y tengelyen való sebesség negatív-e

        var tempVelocity = rb.velocity.y;

        if (!isGrounded && tempVelocity < 0f)
        {
            animator.SetBool("Fall", true);
        }
        else
        {
            animator.SetBool("Fall", false);
        }

        //zuhanási sebesség maximalizálása

        if (rb.velocity.y < -20f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -20f);
        }

        // Bang animáció kezelése, (az utolsó pozícióból ahol az y-sebesség nemnegatív volt kivonjuk az utolsó negatív pozíciót, ezt vizsgáljuk milyen nagyságú)
        // minden y pozíció 100%-ban pozitív szám

        if (rb.velocity.y >= 0)
        {
            lastPosY = rb.position.y;
            currPosY = rb.position.y;
        }
        else
        {
            currPosY = rb.position.y;
        }

        if (lastPosY - currPosY > maxHeightWithoutBang)
        {
            animator.SetBool("Bang", true);
        }

        //a player materialjanak állítása, aszerint hogy a földön van-e (így pattan vissza a falról, amikor éppen ugrást végez) 

        if(!isGrounded && !is45)
        {
            rb.sharedMaterial = bounceMat;
        } 
        else
        {
            rb.sharedMaterial = normalMat;
        }

        //a space nyomva tartása alatt növeljük az ugrás erõsségét (minimum ugrás erõssége 4f)

        if (Input.GetKey("space") && canJump && isGrounded)
        {
            animator.SetBool("Bang", false);
            if (jumpValue < 4f)
            {
                jumpValue = 4f;
            }
            jumpValue += 0.05f;
        }

        if (Input.GetKeyDown("space") && isGrounded)
        {
            animator.SetBool("Bang", false);

            if (!isIcy)
            {
                rb.velocity = new Vector2(0.0f,0.0f);

            }

        }

        // ha elértük a miximum ugrási sebességet akkor a karakter elugrik

        if (jumpValue >= 25f && isGrounded)
        {
            rb.sharedMaterial = bounceMat;
            animator.SetBool("Charging", false);
            float tempx = moveInput;
            float tempy = jumpValue;
            rb.velocity = new Vector2((tempx==0?0:(tempx>0?tempx+5:tempx-5)), tempy);
            Invoke("ResetJump", 0.2f);
        }

        //animáció kezelés
        if (jumpValue > 0)
        {
            animator.SetBool("Charging", true);
        }

        // ha elengedjük a space-t mielõtt a maximumra növekedne az ugrási sebesség, akkor a jumpValue aktuális értékével ugrik el
        if (Input.GetKeyUp("space"))
        {
            canJump = true;
            animator.SetBool("Charging", false);
            rb.sharedMaterial = bounceMat;
            if (isGrounded)
            {
                rb.velocity = new Vector2((moveInput==0?0:(moveInput>0?moveInput+5:moveInput-5)), jumpValue);
                animator.SetBool("Charging", false);
            }

        }

        //lenullázzuk az ugrás paramétereit
        if (!isGrounded)
        {
            animator.SetBool("Charging", false);
            jumpValue = 0.0f;
            canJump = true;
        }

    }

    //ez a függvény egy segédfüggvény a Jump() metódushoz, lenullázza az ugrás paramétereit

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0.0f;
        animator.SetBool("Charging", false);
    }

    //ez a függvény játsza a karakter hangjait

    void AudioManager()
    {/*
        bool bounced = animator.GetBool("Bounced");
        bool grounded = animator.GetBool("Grounded");
        bool bang = animator.GetBool("Bang");

        if(bang && grounded)
        {
            audioSource.clip = BangAudio;
            audioSource.Play();
        }*/
    }

        //ezzel a függvénnyel vizualizálom, hogy hol vannak a Ground/Sticky/Icy/Bounce-Check föld/fal detektálást segítõ láthatatlan síkodomjai

        void OnDrawGizmosSelected()
    {

        //groundcheck
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.8f, 0.05f));
        //bouncecheck
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.14f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.14f));
        //45check
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f));
    }

}
