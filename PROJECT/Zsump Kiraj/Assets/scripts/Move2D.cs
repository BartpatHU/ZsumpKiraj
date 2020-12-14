using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Move2D : MonoBehaviour
{
    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public LayerMask groundMask;

    public PhysicsMaterial2D bounceMat, normalMat;
    public bool canJump = true;
    public float jumpValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Groundcheck();
        Move();
        Jump();
        
    }
    void Groundcheck()
    {
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(0.9f, 0.4f), 0f, groundMask);
    }

    void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if(jumpValue == 0.0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }

    }

    void Jump()
    {
        if(!isGrounded)
        {
            rb.sharedMaterial = bounceMat;
        } 
        else
        {
            rb.sharedMaterial = normalMat;
        }

        if (Input.GetKey("space") && canJump && isGrounded)
        {
            jumpValue += 0.02f;
        }

        if (Input.GetKeyDown("space"))
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        if (jumpValue >= 15f && isGrounded)
        {
            float tempx = moveInput * walkSpeed;
            float tempy = jumpValue;
            rb.velocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0.2f);
        }
        if (Input.GetKeyUp("space"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0.0f;
            }
            canJump = true;
        }
    }
    void ResetJump()
    {
        canJump = false;
        jumpValue = 0.0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.01f), new Vector2(0.7f, 0.01f));
    }

}
