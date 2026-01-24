using UnityEngine;

public class GolemMovement : EnemyMovement
{
    


    public override void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectPoint.position, detectRadius, pLayer);



        if (hits.Length > 0)
        {
            player = hits[0].transform;
            if (Vector2.Distance(stat.atkPoint.position, player.position) <= stat.atkRange && canMove && stat.canAttack)
            {

                if (stat.atkTimer <= 0)
                {
                    ChangeState(EnemyState.Attacking);
                }
                else if (stat.atkTimer > 0 && enemyState != EnemyState.Attacking)
                {
                    ChangeState(EnemyState.Idle);
                }

            }
            else if (Vector2.Distance(stat.atkPoint.position, player.position) > stat.atkRange && enemyState != EnemyState.Attacking && canMove)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            if (!jumping)
            {
                rb.linearVelocity = Vector2.zero;
                ChangeState(EnemyState.Idle);
            }
            else
            {
                ChangeState(EnemyState.Idle);
            }

        }
    }

}
