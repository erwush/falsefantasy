using UnityEngine;
using System.Collections;

public class Skill5 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isActive;
    public float dmg;
    private float timer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public IEnumerator Starting()
    {
        yield return new WaitForSeconds(0.5f);
        timer = 6f;
        StartCoroutine(DealDamage());
    }

    public IEnumerator DealDamage()
    {
        while (timer > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().HealthChange(-dmg);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
