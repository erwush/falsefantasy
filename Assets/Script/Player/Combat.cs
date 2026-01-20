using UnityEngine;

public class Combat : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float armorDmg;
    public float atk;
    public float atkCd;
    public float atkTimer;
    public float atkRange;
    [SerializeField] private Animator anim;
    [SerializeField] private BarController healthBar;
    [SerializeField] private Transform atkPoint;
    [SerializeField] private LayerMask eLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
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
            foreach (Collider2D enemy in enemies)
            {
                enemyCombat = enemies[0].gameObject.GetComponent<EnemyCombat>();
                demeg = atk;
                enemies[0].GetComponent<EnemyCombat>().HealthChange(-demeg);
                Debug.Log(demeg);
            }
        }
    }

    public void FinishAttack()
    {
        if (anim.GetBool("Attack1") == true && Input.GetButtonUp("Attack"))
        {
            anim.SetBool("Attack1", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }
}
