using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using System;

public class Move2D : MonoBehaviour
{
    //s�t�l�shoz sz�ks�ges v�ltoz�k

    public float walkSpeed = 5f;
    public float currSpeed = 4f;
    public float moveInput;
    public Vector2 movement, movementOrder;
    public bool moveLeft = false, moveRight = false;

    public bool LEFUT = false;

    // ugr�shoz sz�ks�ges dolgok

    public bool canJump = true;
    public float jumpValue = 0.0f;

    //a talaj detekt�l�s�hoz sz�ks�ges v�ltoz�k

    public bool isGrounded, isGrounded2;
    public bool isIcy;
    public bool isSticky;
    public bool is45;
    public LayerMask groundMask;
    public LayerMask icyMask;
    public LayerMask stickyMask;
    public LayerMask fourtyFiveMask;

    //seg�dv�ltoz�k

    public float lastPosY;
    public float lastPosX;
    public float currPosY;
    public float maxHeightWithoutBang = 11;

    public bool left;
    public bool right;


    public float XVELOCITY;


    //k�telez� komponensek, tulajdons�gok

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    public PhysicsMaterial2D bounceMat, normalMat;
    public AudioClip JumpAudio;
    public AudioClip LandAudio;
    public AudioClip BangAudio;
    public AudioClip BounceAudio;
    public AudioSource audioSource;

    // A Start az els� k�pkocka friss�l�s el�tt h�v�dik meg
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Az Update k�pkock�nk�nt egyszer h�v�dik meg
    void Update()
    {


    }

    // A FixedUpdate annyiszor h�v�dik meg amennyit be�ll�tunk neki az Edit >> Project Settings >> Time men�pont alatt
    // Az Update h�v�sa kev�snek bizonyult, ez�rt kellett a FixedUpdate >> sim�bb j�t�kmenet, dinamikusabb anim�ci�k

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

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }

    void StuckCharacter()
    {
        if(!isGrounded && rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            animator.SetBool("Grounded", true);
        }
    }

    // ez a f�ggv�ny megn�zi hogy a f�ld�n van-e a karakter

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.8f, 0.05f), 0f, groundMask);
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && !isGrounded2)
        {
            audioSource.clip = LandAudio;
            audioSource.Play();
        }

        isGrounded2 = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.8f, 0.05f), 0f, groundMask);

    }

    //ez a f�ggv�ny megn�zi hogy jeges f�ld�n van-e a karakter

    void IcyCheck()
    {
        isIcy = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.8f, 0.05f), 0f, icyMask);
    }

    //ez a f�ggv�ny megn�zi, hogy ragad�s f�ld�n van-e a karakter

    void StickyCheck()
    {
        isSticky = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.04f), new Vector2(0.8f, 0.05f), 0f, stickyMask);
    }

    //ez a f�ggv�ny megn�zi, hogy 45-fokos f�ld�n van-e a karakter

    void FourtyFiveCheck()
    {
        bool left45 = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f), 0f, fourtyFiveMask);
        bool right45 = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.47f, gameObject.transform.position.y - 0.04f), new Vector2(0.05f, 0.05f), 0f, fourtyFiveMask);

        if(right45 || left45)
        {
            is45 = true;
            rb.sharedMaterial = normalMat;
        }
        else
        {
            is45 = false;
        }

    }

    //ez a f�ggv�ny azt vizsg�lja hogy az aktu�lis ugr�s k�zben pattant-e m�r vissza falr�l a karakter (anim�ci� hoz sz�ks�ges)

    void BounceCheck()
    {
        left = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.12f), 0f, groundMask);
        right = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.45f, gameObject.transform.position.y + 0.57f), new Vector2(0.03f, 1.12f), 0f, groundMask);

        if((right || left) && !isGrounded && rb.velocity.x != 0)
        {
            animator.SetBool("Bounced", true);
            audioSource.clip = BounceAudio;
            audioSource.Play();
        }

        if (isGrounded)
        {
            animator.SetBool("Bounced", false);
        }

    }

    //ebben a f�ggv�nyben van megval�s�tva a horizont�lis mozg�s

    void Move()
    {
        // sebess�g be�ll�t�sa, sprite renderer y tengelyen val� t�kr�z�se, hogy mindg arra n�zzena karakter amerra mozogni szeretne vagy amerre utolj�ra mozgott

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

        //normal s�t�l�s

        if (jumpValue == 0.0f && isGrounded && !isIcy && !isSticky)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            rb.velocity = new Vector2(moveInput, rb.velocity.y);
        }

        //ragad�s s�t�l�s

        if (jumpValue == 0.0f && isGrounded && !isIcy && isSticky)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", rb.velocity.x);
        }

        //jeges s�t�l�s

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


        if (isGrounded && isIcy && !isSticky)
        {

            if (jumpValue == 0f)
            {
                if (moveLeft && !moveRight)
                {
                    movementOrder.x = -walkSpeed - 2;
                }
                else if (moveRight && !moveLeft)
                {
                    movementOrder.x = walkSpeed + 2;
                }


                if (moveLeft && movement.x > movementOrder.x)
                {
                    movement.x -= currSpeed * Time.fixedDeltaTime;
                }
                if (moveRight && movement.x < movementOrder.x)
                {
                    movement.x += currSpeed * Time.fixedDeltaTime;
                }
            }


            
            movement.y = rb.velocity.y;
            rb.velocity = movement;

           if((left || right) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && XVELOCITY == 0)
            {
                LEFUT = true;
                movement.x = 0f;
            } else
            {
                LEFUT = false;
            }
        }



    }

    //ebben a f�ggv�nyben van megval�s�tva a vertik�lis mozg�s

    void Jump()
    {

        // a fall anim�ci� kezel�se a szerint hogy az y tengelyen val� sebess�g negat�v-e

        var tempVelocity = rb.velocity.y;

        if (!isGrounded && tempVelocity < 0f)
        {
            animator.SetBool("Fall", true);
        }
        else
        {
            animator.SetBool("Fall", false);
        }

        //zuhan�si sebess�g maximaliz�l�sa

        if (rb.velocity.y < -20f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -20f);
        }

        // Bang anim�ci� kezel�se, (az utols� poz�ci�b�l ahol az y-sebess�g nemnegat�v volt kivonjuk az utols� negat�v poz�ci�t, ezt vizsg�ljuk milyen nagys�g�)
        // minden y poz�ci� 100%-ban pozit�v sz�m

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
            if (isGrounded)
            {
                audioSource.clip = BangAudio;
                audioSource.Play();
            }
        }

        //a player materialjanak �ll�t�sa, aszerint hogy a f�ld�n van-e (�gy pattan vissza a falr�l, amikor �ppen ugr�st v�gez) 

        if(!isGrounded && !is45 && rb.velocity.y!=0f)
        {
            rb.sharedMaterial = bounceMat;
        } 
        else
        {
            rb.sharedMaterial = normalMat;
        }

        //a space nyomva tart�sa alatt n�velj�k az ugr�s er�ss�g�t (minimum ugr�s er�ss�ge 4f)

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

        // ha el�rt�k a miximum ugr�si sebess�get akkor a karakter elugrik

        if (jumpValue >= 25f && isGrounded)
        {
            audioSource.clip = JumpAudio;
            audioSource.Play();

            rb.sharedMaterial = bounceMat;
            animator.SetBool("Charging", false);
            float tempx = moveInput;
            float tempy = jumpValue;
            if (!isIcy)
            {
                rb.velocity = new Vector2((tempx == 0 ? 0: (tempx > 0 ? tempx + 5: tempx - 5)), tempy);
            }
            else
            {
                rb.velocity = new Vector2((tempx == 0 ? 0 + rb.velocity.x : (tempx > 0 ? tempx + 5 + rb.velocity.x : tempx - 5 + rb.velocity.x)), tempy);
            }
            Invoke("ResetJump", 0.2f);
        }

        //anim�ci� kezel�s
        if (jumpValue > 0)
        {
            animator.SetBool("Charging", true);
        }

        // ha elengedj�k a space-t miel�tt a maximumra n�vekedne az ugr�si sebess�g, akkor a jumpValue aktu�lis �rt�k�vel ugrik el
        if (Input.GetKeyUp("space"))
        {

            float tempx = moveInput;
            float tempy = jumpValue;

            canJump = true;
            animator.SetBool("Charging", false);
            rb.sharedMaterial = bounceMat;
            if (isGrounded)
            {
                audioSource.clip = JumpAudio;
                audioSource.Play();
                if (!isIcy)
                {
                    rb.velocity = new Vector2((tempx == 0 ? 0 : (tempx > 0 ? tempx + 5 : tempx - 5)), tempy);
                }
                else
                {
                    rb.velocity = new Vector2((tempx == 0 ? 0 + rb.velocity.x : (tempx > 0 ? tempx + 5 + rb.velocity.x : tempx - 5 + rb.velocity.x)), tempy);
                }
                animator.SetBool("Charging", false);
            }

        }

        //lenull�zzuk az ugr�s param�tereit
        if (!isGrounded)
        {
            animator.SetBool("Charging", false);
            jumpValue = 0.0f;
            canJump = true;
        }

    }

    //ez a f�ggv�ny egy seg�df�ggv�ny a Jump() met�dushoz, lenull�zza az ugr�s param�tereit

    void ResetJump()
    {
        canJump = false;
        jumpValue = 0.0f;
        animator.SetBool("Charging", false);
    }

    //ez a f�ggv�ny j�tsza a karakter hangjait

    void AudioManager()
    {

    }

        //ezzel a f�ggv�nnyel vizualiz�lom, hogy hol vannak a Ground/Sticky/Icy/Bounce-Check f�ld/fal detekt�l�st seg�t� l�thatatlan s�kodomjai

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
