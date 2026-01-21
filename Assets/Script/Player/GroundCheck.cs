using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public Collider2D groundCheck;
    public Movement movementScript;
    // public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GameObject.FindWithTag("Player").GetComponent<Movement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Solid")
        {
            movementScript.isGrounded = true;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Solid")
        {
            movementScript.isGrounded = false;

        }
    }
}
