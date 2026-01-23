using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform checkpoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            // Debug.Log("checkpoint");
            checkpoint = other.transform;
        }
    }
    
    public void Respawn()
    {
        transform.position = new Vector3(checkpoint.position.x, checkpoint.position.y + 7, checkpoint.position.z);
    }
}
