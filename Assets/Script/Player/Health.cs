using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHp;
    private Checkpoint checkpoint;
    private Combat combat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpoint = GetComponent<Checkpoint>();
        combat = GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HealthChange(float amount)
    {
        if(amount < 0 && !combat.isParry)
        {
            hp += amount;   
        } else if (combat.isParry)
        {
            combat.parryTimer = 0f;
        }
        if(amount> 0)
        {
            hp += amount;
        }
        if (hp <= 0)
        {
            hp = maxHp;
            checkpoint.Respawn();
        }
        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }
}
