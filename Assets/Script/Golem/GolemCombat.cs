using UnityEngine;

public class GolemCombat : EnemyCombat
{
    public float knockStrength;
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
            hits[0].GetComponent<Health>().HealthChange(-demeg);
            hits[0].GetComponent<Movement>().Jump(knockStrength, false);
            Debug.Log("demeg musuh:" + demeg);

        }
    }

    public override void HealthChange(float hpAmount, float armorAmount)
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
    }



}
