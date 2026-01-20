using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float armor;
    public float atk;
    public float atkInterval;
    [SerializeField] private BarController healthBar;
    [SerializeField] private BarController armorBar;
    [SerializeField] private Transform atkPoint;
    [SerializeField] private LayerMask pLayer;
    [SerializeField] private Combat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void HealthChange(float amount)
    {
        hp += amount;
        healthBar.UpdateBar(hp, maxHp);
    }
}
