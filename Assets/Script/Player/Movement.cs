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
    public float knockback;
    private Rigidbody2D rb;
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
        if (canMove && Input.GetButtonDown("Dash") && dashToken > 0 && dashTimer <= 0f)
        {
            StartCoroutine(Dash());
        }
        if (dashTimer > 0f)
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
        if ((canMove && Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && justJumped) && Time.timeScale != 0)
        {
            if (isGrounded)
            {
                Jump(jumpStrength, true);
            }
            else if (justJumped == true && doubleJumped == false)
            {
                DoubleJump(jumpStrength);
            }
        }
    }

    public void Jump(float jumpPower, bool fromJump)
    {
        if (fromJump)
        {
            justJumped = true;
            doubleJumped = false;
        }
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public void DoubleJump(float jumpPower)
    {
        // Debug.Log("DoubleJumped");
        justJumped = false;
        doubleJumped = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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

  public void Knockback(Transform enemy, float knockbackForce, float knockDuration = 0.5f)
    {
        canMove = false;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(KnockbackCounter(knockDuration));
    }

    private IEnumerator KnockbackCounter(float knockDuration)
    {

        float elapsed = 0f;

        while (elapsed < knockDuration)
        {
            // perlahan turunkan velocity menuju 0
            rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                Vector2.zero,
                Time.deltaTime * 2.5f // 5f bisa diubah jadi lebih besar/lebih kecil
            );

            elapsed += Time.deltaTime;
            yield return null; // tunggu 1 frame
        }
        canMove = true;
    }



    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}