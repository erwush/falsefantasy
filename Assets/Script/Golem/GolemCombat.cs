using UnityEngine;
using System.Collections;

public class GolemCombat : EnemyCombat
{
    public float knockStrength;
    public GameObject quakeEffect;
    private bool isCharged;
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

            if (isCharged)
            {
                StartCoroutine(KnockUp(hits[0].transform.position));
                hits[0].GetComponent<Movement>().Jump(knockStrength, false);
                hits[0].GetComponent<Health>().HealthChange(-demeg * 1.15f);
            }
            else if (!isCharged)
            {

                hits[0].GetComponent<Health>().HealthChange(-demeg);
            }

        }
    }

    //hits is the psoitio
    IEnumerator KnockUp(Vector2 pos)
    {
        GameObject quake = Instantiate(quakeEffect, new Vector2(pos.x, pos.y-1.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(quake);
    }


    public void RandomAttack()
    {
        int r = Random.Range(0, 2);

        if (r == 0)
        {
            isCharged = true;
        }
        else if (r == 1)
        {
            isCharged = false;
        }
    }

    public override void HealthChange(float hpAmount)
    {
        if (hpAmount < 0)
        {
            StartCoroutine(HurtAnim());
        }
        atkCd = hp / 100f;


        hp += hpAmount;
        healthBar.UpdateBar(hp, maxHp);

    }





}
