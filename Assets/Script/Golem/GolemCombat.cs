using UnityEngine;
using System.Collections;

public class GolemCombat : EnemyCombat
{
    public float knockStrength;
    public GameObject quakeEffect;

    private bool[] isCharged;
    private Collider2D kolleder;

    void Start()
    {
        skillAmount = 2;
        kolleder = GetComponent<Collider2D>();
        isCharged = new bool[2];
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        selfSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = selfSprite.material;
        canAttack = true;
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(atkPoint.position, atkRange, pLayer);
        atkTimer = atkCd;
        if (PlayerCombat == null)
        {
            PlayerCombat = hits[0].gameObject.GetComponent<Combat>();
        }
        if (hits.Length > 0)
        {
            float demeg;
            demeg = atk;

            if (isCharged[0])
            {
                StartCoroutine(Jump(hits[0].transform.position, demeg));
                StartCoroutine(KnockUp(hits[0].transform.position));
                hits[0].GetComponent<Movement>().Jump(knockStrength, false);
                hits[0].GetComponent<Health>().HealthChange(-demeg * 1.15f);

            }
            else if (isCharged[1])
            {

                StartCoroutine(KnockUp(hits[0].transform.position));
                hits[0].GetComponent<Movement>().Jump(knockStrength, false);
                hits[0].GetComponent<Health>().HealthChange(-demeg * 1.15f);
                StartCoroutine(Dash());
            }
            else if (!isCharged[0] && !isCharged[1])
            {

                hits[0].GetComponent<Health>().HealthChange(-demeg);
            }

        }
    }

    //hits is the psoitio
    IEnumerator KnockUp(Vector2 pos)
    {
        GameObject quake = Instantiate(quakeEffect, new Vector2(pos.x, pos.y - 1.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(quake);
    }

    IEnumerator Dash()
    {
        //move backward
        yield return new WaitForSeconds(0.75f);
        movement.ChangeState(EnemyState.Idle);
        movement.canMove = true;
        canAttack = false;
        movement.unstop = true;
        float dir = Mathf.Sign(transform.localScale.x);
        kolleder.excludeLayers = 0;
        // mundur dulu
        rb.linearVelocity = new Vector2(-dir * movement.dashStrength, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.1f);
        // terjang
        rb.linearVelocity = new Vector2(dir * movement.dashStrength, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.25f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, atkRange * 1.5f, pLayer);
        if (hits.Length > 0)
        {
            rb.linearVelocity = Vector2.zero;
            hits[0].GetComponent<Health>().HealthChange(-atk * 1.15f);
            hits[0].GetComponent<Movement>().Knockback(transform, knockStrength);
        }
        yield return new WaitForSeconds(0.1f);
        movement.unstop = false;
        canAttack = true;
        kolleder.excludeLayers = pLayer;

    }
    
    



    IEnumerator Jump(Vector2 pos, float demeg)
    {
        movement.unstop = true;
        canAttack = false;
        yield return new WaitForSeconds(0.75f);
        transform.position = new Vector2(pos.x, pos.y + 7f);
        yield return new WaitForSeconds(0.25f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, atkRange * 3.5f, pLayer);
        if (hits.Length > 0)
        {
            hits[0].GetComponent<Movement>().Jump(knockStrength + 10, false);
            hits[0].GetComponent<Health>().HealthChange(-demeg * 1.15f);
            StartCoroutine(KnockUp(hits[0].transform.position));
        }
        yield return new WaitForSeconds(0.5f);
        movement.unstop = false;
        canAttack = true;

    }


    public void RandomAttack()
    {
        int r = Random.Range(0, 3);

        if (r == 0)
        {
            isCharged[0] = false;
            isCharged[1] = false;
        }
        else if (r == 1)
        {
            isCharged[0] = true;
        }
        else if (r == 2)
        {
            isCharged[1] = true;
        }
    }

    public override void HealthChange(float hpAmount)
    {
        if (hpAmount < 0)
        {
            StartCoroutine(HurtAnim());
        }
        atkCd = hp / 100f;
        if (atkCd <= 1)
        {
            atkCd = 1;
        }


        hp += hpAmount;
        healthBar.UpdateBar(hp, maxHp);

    }





}
