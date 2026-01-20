using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class Movement : MonoBehaviour
{
    public float spd;
    public float dashStrength;
    public float jumpStrength;
    public Rigidbody2D rb;
    public float knockback;
    [SerializeField] private Animator anim;
    [SerializeField] private bool canMove;
    [SerializeField] private int facingDirection = 1;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isGrounded;
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

        if(direct.x > 0 && facingDirection < 0 || direct.x < 0 && facingDirection > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingDirection *= -1;
         transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}