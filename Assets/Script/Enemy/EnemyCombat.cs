using System.Collections;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float armor;
    public float maxArmor;
    public float atk;
    public float atkTimer;
    public float atkCd;
    public float atkRange;
    public float armorTimer;
    public float finalDuration;
    public bool isHit;
    public Material flashHit;
    public Material defaultMaterial;
    protected Combat PlayerCombat;
    protected SpriteRenderer selfSprite;
    protected EnemyMovement movement;
    protected Coroutine armorCor;
    protected Coroutine waitCor;
    protected Coroutine finalCor;
    

    [SerializeField] protected BarController healthBar;
    [SerializeField] protected Animator anim;
    [SerializeField] protected BarController armorBar;
    [SerializeField] protected Transform atkPoint;
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
        armorBar.UpdateBar(armor, maxArmor);
        if (atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        if (isHit)
        {
            if (armorCor != null)
            {
                StopCoroutine(armorCor);
                armorCor = null;
            }
            if (waitCor == null)
            {
                waitCor = StartCoroutine(ArmorWait());
            }
        }
        else if (!isHit)
        {
            if (waitCor != null)
            {
                StopCoroutine(waitCor);
                waitCor = null;
            }
        }
    }

    public IEnumerator ArmorRecovering()
    {
        while (armor < maxArmor && !isHit)
        {
            armor += maxArmor * 0.2f;
            armorBar.UpdateBar(armor, maxArmor);
            if (armor <= maxArmor)
            {
                armor = maxArmor;
            }
            yield return new WaitForSeconds(1f);
            if (armor >= maxArmor)
            {
                armor = maxArmor;
                armorCor = null;
                StopCoroutine(ArmorRecovering());

            }
        }
    }

    public IEnumerator ArmorWait()
    {
        yield return new WaitForSeconds(3f);
        isHit = false;
        if (armorCor == null)
        {
            armorCor = StartCoroutine(ArmorRecovering());
        }
        waitCor = null;
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
            Debug.Log("demeg musuh:" + demeg);

        }
    }

    public virtual void HealthChange(float hpAmount, float armorAmount)
    {
        if(hpAmount < 0)
        {
            StartCoroutine(HurtAnim());
        }
        armor += armorAmount;
        if (armor <= 0)
        {
            hp += hpAmount * 1.5f;
            if (finalCor == null)
            {
                finalCor = StartCoroutine(FinalDuration());
            }
        }
        else
        {
            hp += hpAmount;
        }

        healthBar.UpdateBar(hp, maxHp);
        armorBar.UpdateBar(armor, maxArmor);
    }

    protected IEnumerator FinalDuration()
    {

        StopCoroutine(ArmorRecovering());
        selfSprite.color = Color.red;
        yield return new WaitForSeconds(finalDuration);
        armor = maxArmor;
        armorBar.UpdateBar(armor, maxArmor);
        selfSprite.color = Color.white;
        finalCor = null;

    }



    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }

    IEnumerator HurtAnim()
    {
        selfSprite.material = flashHit;
        yield return new WaitForSeconds(0.2f);
        selfSprite.material = defaultMaterial;
    }
}
