using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combat : MonoBehaviour
{
    public float atk;
    public float finalStack;
    public float maxFinal = 10;
    public float atkCd;
    public float atkTimer;
    public float atkRange;
    public bool isFinal;
    public float finalDuration;
    [HideInInspector] public int finalHit;
    [SerializeField] private Animator anim;
    [SerializeField] private BarController healthBar;
    [SerializeField] private Transform atkPoint;
    [SerializeField] private LayerMask eLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        maxFinal = 10;
    }



    void FixedUpdate()
    {
        if (Input.GetButtonDown("Attack") && atkTimer <= 0)
        {
            Attack();
        }
    }

    void Update()
    {
        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        if (isFinal && finalDuration > 0)
        {
            finalDuration -= Time.deltaTime;
        }
    }

    void Attack()
    {
        anim.SetBool("Attack1", true);
        atkTimer = atkCd;
    }

    public void ApplyDamage()
    {
        float demeg;
        EnemyCombat enemyCombat;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(atkPoint.position, atkRange, eLayer);
        if (enemies.Length > 0)
        {
            // Debug.Log("kena");
            foreach (Collider2D enemy in enemies)
            {
                enemyCombat = enemies[0].gameObject.GetComponent<EnemyCombat>();
                demeg = atk;
                enemies[0].GetComponent<EnemyCombat>().HealthChange(-demeg);
                if (!isFinal)
                {
                    finalStack += 1;
                    if(finalStack >= maxFinal)
                    {
                        StartCoroutine(Finalization(3f, 3));
                    }

                } else
                {
                    finalHit += 1;
                }
                enemies[0].GetComponent<EnemyCombat>().isHit = true;
                // enemies[0].GetComponent<EnemyCombat>().StopCoroutine(enemies[0].GetComponent<EnemyCombat>().ArmorRecovering());

            }
        }
    }

    public void FinishAttack()
    {
        if (anim.GetBool("Attack1") == true && !Input.GetButtonDown("Attack"))
        {
            anim.SetBool("Attack1", false);
        }
    }

    public IEnumerator Finalization(float duration, float dmgBoost)
    {
        finalDuration = duration;
        atk *= dmgBoost;
        isFinal = true;
        yield return new WaitUntil(() => finalDuration <= 0);
        atk /= dmgBoost;
        isFinal = false;
        finalStack = 0;
        finalHit = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }
}
