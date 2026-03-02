using UnityEngine;

public class Pazel : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging;

    public Transform correctPlace;   // tempat yang benar
    public float snapDistance = 0.5f;
    public bool isPlaced;
    public GameObject obj;
    public GameObject arrow;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (!isPlaced)
        {
            isDragging = true;
        }
    }
    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            worldPos.z = 0;
            transform.position = worldPos;

            arrow.SetActive(true);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        arrow.SetActive(false);
        // cek jarak ke tempat benar
        float distance = Vector2.Distance(transform.position, correctPlace.position);

        if (distance < snapDistance)
        {
            obj.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            Destroy(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}