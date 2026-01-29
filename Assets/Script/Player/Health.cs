using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHp;
    private Checkpoint checkpoint;
    private Combat combat;
    private Movement movement;
    private bool iFrame;
    private Collider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpoint = GetComponent<Checkpoint>();
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HealthChange(float amount)
    {
        if (!iFrame)
        {
            if (amount < 0 && !combat.isParry)
            {
                hp += amount;
                StartCoroutine(InvFrame());
                if (hp <= 0)
                {
                    StartCoroutine(Death());
                }
            }
            else if (combat.isParry)
            {
                combat.parryTimer = 0f;
            }
        }
        if (amount > 0)
        {
            hp += amount;
        }

        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    IEnumerator InvFrame()
    {
        iFrame = true;
        yield return new WaitForSeconds(0.01f);
        iFrame = false;
    }

    public IEnumerator Death()
    {
        iFrame = true;
        movement.Jump(25f, false);
        col.isTrigger = true;
        yield return new WaitForSeconds(1.15f);
        hp = maxHp;
        col.isTrigger = false;
        checkpoint.Respawn();
        iFrame = false;

    }

}
