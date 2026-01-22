using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class Movement : MonoBehaviour
{
    public float spd;
    public bool isGrounded;
    public float jumpStrength;
    public Rigidbody2D rb;
    public float knockback;
    private Coroutine dashCor;
    [SerializeField] private int dashToken;
    [SerializeField] private float dashTimer;
    [SerializeField] private Animator anim;
    [SerializeField] private bool canMove;
    [SerializeField] private int facingDirection = 1;
    [SerializeField] private Collider2D groundCheck;
    [SerializeField] private bool doubleJumped;
    [SerializeField] private bool justJumped;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private InputActionReference input;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
        anim = GetComponent<Animator>();


    }

    void Update()
    {
        if (Input.GetButtonDown("Dash") && dashToken > 0 && dashTimer <= 0f)
        {
            StartCoroutine(Dash());
        }
        if(dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }
        if (dashToken < 2)
        {
            if (dashCor == null)
            {
                dashCor = StartCoroutine(DashRecovery());
            }
        }
        //*JUMP
        if ((Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && justJumped) && Time.timeScale != 0)
        {
            if (isGrounded)
            {
                justJumped = true;
                doubleJumped = false;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            }
            else if (justJumped == true && doubleJumped == false)
            {
                Debug.Log("DoubleJumped");
                justJumped = false;
                doubleJumped = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator DashRecovery()
    {
        while (dashToken < 2)
        {
            yield return new WaitForSeconds(0.5f);
            dashToken += 1;
        }
        dashCor = null;
    }


    void FixedUpdate()
    {
        Vector2 direct = input.action.ReadValue<Vector2>();
        if (canMove)
        {
            rb.linearVelocity = new Vector2(direct.x * spd, rb.linearVelocity.y);
        }
        if (direct.x != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (direct.x > 0 && facingDirection < 0 || direct.x < 0 && facingDirection > 0)
        {
            Flip();
        }
    }

    //Coroutine for Dashing
    public IEnumerator Dash()
    {

        float defSpd = spd;
        spd *= 5;
        dashToken -= 1;
        yield return new WaitForSeconds(0.1f);
        spd = defSpd;
        dashTimer = 0.2f;
    }



    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}