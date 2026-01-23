using System.Collections;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    public float hp;
    public float maxHp;

    public float atk;
    public float atkTimer;
    public float atkCd;
    public float atkRange;

    public bool isHit;
    public Material flashHit;
    public Transform atkPoint;
    public Material defaultMaterial;
    protected Combat PlayerCombat;
    protected SpriteRenderer selfSprite;
    protected EnemyMovement movement;
    [SerializeField] protected BarController healthBar;
    [SerializeField] protected Animator anim;
    [SerializeField] protected LayerMask pLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        selfSprite = GetComponent<SpriteRenderer>();
        defaultMaterial = selfSprite.material;

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





    public virtual void FinishAttacking()
    {
        if (anim.GetBool("isAttacking1") == true)
        {
            anim.SetBool("isAttacking1", false);


        }
    }

    public virtual void Attack()
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

    public virtual void HealthChange(float hpAmount)
    {
        if (hpAmount < 0)
        {
            StartCoroutine(HurtAnim());
        }


        hp += hpAmount;


        healthBar.UpdateBar(hp, maxHp);

    }





    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }

    protected IEnumerator HurtAnim()
    {
        selfSprite.material = flashHit;
        yield return new WaitForSeconds(0.2f);
        selfSprite.material = defaultMaterial;
    }
}
