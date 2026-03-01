using UnityEngine;

public class KrocoMovement : EnemyMovement
{
  
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

  
}





