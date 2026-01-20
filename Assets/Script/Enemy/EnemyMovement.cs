using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float spd;
    public float jumpStrength;
    public float dashStrength;
    public Rigidbody2D rb;
    [SerializeField] int facingDirection = 1;
    public float knockback;
    [SerializeField] private bool canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
