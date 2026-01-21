using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float spd;
    public float jumpStrength;
    public float dashStrength;
    public Rigidbody2D rb;
    public EnemyState enemyState;
    public float detectRadius;
    public EnemyCombat stat;
    public Transform detectPoint;
    public LayerMask pLayer;
    public float knockback;
    private Animator anim;
    private Transform player;
    private SpriteRenderer selfSprite;
    [SerializeField] int facingDirection = 1;
    [HideInInspector] public bool canMove;
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
        else if (enemyState == EnemyState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;

        }
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
            //chagne color to red
            selfSprite.color = Color.red;
        }

    }

    void Chase()
    {
        if (player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * spd;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectPoint.position, detectRadius, pLayer);



        if (hits.Length > 0)
        {
            player = hits[0].transform;
            if (Vector2.Distance(transform.position, player.position) <= stat.atkRange && stat.atkTimer <= 0 && canMove)
            {

                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.position) > stat.atkRange && enemyState != EnemyState.Attacking && canMove)
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

    public void ChangeState(EnemyState newState)
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
}



public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}