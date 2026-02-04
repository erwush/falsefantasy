using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHp;
    private Combat combat;
    private Movement movement;
    private bool iFrame;
    private Collider2D col;
    [SerializeField] private BarController healthBar;
    [HideInInspector] public Transform checkpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combat = GetComponent<Combat>();
        movement = GetComponent<Movement>();
        col = GetComponent<Collider2D>();
        healthBar = GetComponentInChildren<BarController>();
    }

    // Update is called once per frame
    void Update()
    {
        // healthBar.UpdateBar(hp, maxHp);

    }

    public void HealthChange(float amount)
    {
        if (!iFrame)
        {
            if (amount < 0 && !combat.isParry)
            {
                hp += amount;

                if (hp <= 0)
                {
                    StartCoroutine(Death());
                }
                else
                {
                    StartCoroutine(InvFrame());
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
        Respawn();
        iFrame = false;

    }

    public void Respawn()
    {
        transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y + 7, checkpoint.position.z);
    }

}
