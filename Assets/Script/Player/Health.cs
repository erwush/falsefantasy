using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHp;
    private Checkpoint checkpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpoint = GetComponent<Checkpoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HealthChange(float amount)
    {

        hp += amount;
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
