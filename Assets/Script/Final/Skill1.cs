using UnityEngine;
using System.Collections;

public class Skill1 : MonoBehaviour
{
    float spd;
    public Vector3 player;
    Rigidbody2D rb;
    public float dmg;
    public float radius;
    public bool canMove;
    public LayerMask pLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spd = 50f;

        canMove = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move toward player
        if (canMove)
        {
            Vector2 direction = (player - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * spd, direction.y * spd);
        }
        if (Vector2.Distance(transform.position, player) < 0.5f)
        {
            canMove = false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, pLayer);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<Health>().HealthChange(-dmg);
            }
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, pLayer);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<Health>().HealthChange(-dmg);
            }
            Destroy(gameObject);
        }
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
