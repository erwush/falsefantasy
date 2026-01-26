using System.Collections;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    GameObject player;
    public float dmg;
    public GameObject obj;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer == 10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            player = col.gameObject;
            obj.GetComponent<FinalCombat>().StartCoroutine(obj.GetComponent<FinalCombat>().StartFinal(player));
            Destroy(gameObject);
        }
    }

}
