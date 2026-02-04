using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public float atk;
    public float finalStack;
    public float maxFinal = 10;
    public float atkCd;
    public bool isParry;
    public GameObject finalVign;
    public float atkTimer;
    public float atkRange;
    public bool isFinal;
    public float finalDuration;
    public BarController finalBar;
    public float parryTimer;
    [HideInInspector] public int finalHit;
    [SerializeField] private Animator anim;
    [SerializeField] private BarController healthBar;
    [SerializeField] private Transform atkPoint;
    [SerializeField] private LayerMask eLayer;
    [SerializeField] private Movement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        maxFinal = 10;
    }



    void FixedUpdate()
    {
        if (Input.GetButtonDown("Attack") && atkTimer <= 0 && movement.canMove)
        {
            Attack();
        }
        
    }

    void Update()
    {
        if (!isFinal)
        {
            finalBar.UpdateBar(finalStack, 10);
            //  finalBar.slider.direction = Slider.Direction.LeftToRight;
        }
        else
        {
            finalBar.UpdateBar(finalDuration, 5f);
            // finalBar.slider.direction = Slider.Direction.RightToLeft;
        }
        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        if (isFinal && finalDuration > 0)
        {
            finalDuration -= Time.deltaTime;
        }
        if (parryTimer > 0)
        {
            parryTimer -= Time.deltaTime;
        }

        if(parryTimer <= 0 && Input.GetButtonDown("Parry"))
        {
            StartCoroutine(parry());
        }
    }

    IEnumerator parry()
    {
        isParry = true;
        yield return new WaitForSeconds(0.3f);
        isParry = false;
        parryTimer = 1.5f;
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
                    if (finalStack >= maxFinal)
                    {
                        StartCoroutine(Finalization(3f, 3));
                    }

                }
                else
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
        finalVign.SetActive(true);
        finalDuration = duration;
        atk *= dmgBoost;
        isFinal = true;
        yield return new WaitUntil(() => finalDuration <= 0);
        atk /= dmgBoost;
        isFinal = false;
        finalVign.SetActive(false);
        finalStack = 0;
        finalHit = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }
}
