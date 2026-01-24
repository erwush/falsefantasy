using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public float spd;
    public float jumpStrength;
    public float dashStrength;
   

    public EnemyState enemyState;
    public float detectRadius;
    public EnemyCombat stat;
    public Transform detectPoint;
    public LayerMask pLayer;
    public float knockback;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Transform player;
    
    protected SpriteRenderer selfSprite;
    [SerializeField] int facingDirection = 1;
    [HideInInspector] public bool unstop;
    public bool canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<EnemyCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        if (enemyState == EnemyState.Chasing)
        {
            if (canMove)
            {
                Chase();
            }
        }
        else if (enemyState == EnemyState.Attacking && !unstop)
        {
            rb.linearVelocity = Vector2.zero;

        }
        if (!canMove && !unstop)
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
            //chagne color to red
        }

    }

    void Chase()
    {
        // Flip arah hadap
        if ((player.position.x > transform.position.x && facingDirection == -1) ||
            (player.position.x < transform.position.x && facingDirection == 1))
        {
            Flip();
        }

        // Gerak hanya di X
        float dirX = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dirX * spd, rb.linearVelocity.y);

    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public virtual void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectPoint.position, detectRadius, pLayer);



        if (hits.Length > 0)
        {
            player = hits[0].transform;
            if (Vector2.Distance(stat.atkPoint.position, player.position) <= stat.atkRange && stat.atkTimer <= 0 && canMove && stat.canAttack)
            {

                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(stat.atkPoint.position, player.position) > stat.atkRange && enemyState != EnemyState.Attacking && canMove)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);

        }
    }

    public virtual void ChangeState(EnemyState newState)
    {
        //Exit the current animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking1", false);
        }
        //Update current state
        enemyState = newState;

        //Enter the new animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", true);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking1", true);
        }
    }

    void OnDrawGizmosSelected()
    {
        //draw red
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPoint.position, detectRadius);
    }


    public void Jump(float jumpPower)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
}





public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}