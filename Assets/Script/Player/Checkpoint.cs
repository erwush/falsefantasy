using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform checkpoint;
    private bool isChecked;
    private SpriteRenderer sprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isChecked)
        {
            // Debug.Log("checkpoint");
            isChecked = true;
            other.GetComponent<Health>().checkpoint = transform;
            sprite.color = Color.green;
        }
    }
    

}
