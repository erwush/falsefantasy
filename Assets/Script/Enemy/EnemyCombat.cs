using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float armor;
    public float maxArmor;
    public float atk;
    public float atkTimer;
    public float atkCd;
    public float atkRange;
    public float armorRecovery;
    public float armorInterval;
    public float armorTimer;
    public float armorWait;
    public float finalDuration;
    private Combat PlayerCombat;
    private SpriteRenderer selfSprite;
    private EnemyMovement movement;
    private Coroutine armorCor;
    private Coroutine finalCor;
    [SerializeField] private BarController healthBar;
    [SerializeField] private Animator anim;
    [SerializeField] private BarController armorBar;
    [SerializeField] private Transform atkPoint;
    [SerializeField] private LayerMask pLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        selfSprite = GetComponent<SpriteRenderer>();
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
        if (armorTimer > 0)
        {
            armorTimer -= Time.deltaTime;
            if (armorCor != null)
            {
                StopCoroutine(armorCor);
            }
        }
        if (armorTimer <= 0)
        {

            if (armorCor == null)
            {
                armorCor = StartCoroutine(ArmorRecovering());
            }
        }
    }

    public IEnumerator ArmorRecovering()
    {
        while (armor < maxArmor)
        {
            armor += armorRecovery;
            armorBar.UpdateBar(armor, maxArmor);
            yield return new WaitForSeconds(armorInterval);
            if (armor >= maxArmor)
            {
                armorCor = null;
                StopCoroutine(armorCor);
                armor = maxArmor;
            }
        }

    }
    public void FinishAttacking()
    {
        if (anim.GetBool("isAttacking1") == true)
        {
            anim.SetBool("isAttacking1", false);


        }
    }

    public void Attack()
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

    public void HealthChange(float hpAmount, float armorAmount)
    {
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


        armorTimer = armorWait;

    }

    IEnumerator FinalDuration()
    {
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
}
