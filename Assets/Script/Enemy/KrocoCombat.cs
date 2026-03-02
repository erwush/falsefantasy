using System.Collections;
using UnityEngine;

public  class KrocoCombat : EnemyCombat
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        selfSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = selfSprite.material;
        canAttack = true;
        rb =  GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.UpdateBar(hp, maxHp);
        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        // if (isHit)
        // {

        // }
        // else if (!isHit)
        // {
        // }
    }







    public  override void FinishAttacking()
    {
        if (anim.GetBool("isAttacking1") == true)
        {
            anim.SetBool("isAttacking1", false);


        }
    }

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
            hits[0].GetComponent<Health>().HealthChange(-demeg);
            // Debug.Log("demeg musuh:" + demeg);

        }
    }

    public override void HealthChange(float hpAmount)
    {
        if (hpAmount < 0)
        {
            StartCoroutine(HurtAnim());
        }


        hp += hpAmount;


        healthBar.UpdateBar(hp, maxHp);
        if(hp <= 0)
        {
            Destroy(gameObject);
        }

    }





    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }


}
