using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool inArea;
    public GameObject DialogUI;
    public GameObject self;
    public Dialogable dialogable;
    public GameObject keybind;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        self = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (inArea && Input.GetButtonDown("Interact"))
        {
            DialogUI.SetActive(true);
            player.GetComponent<Movement>().canMove = false;
            DialogUI.GetComponent<Dialog>().obj = self;
            DialogUI.GetComponent<Dialog>().ChangeDialog();
        }
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            player = collision.gameObject;
            keybind.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player"){
            inArea = false;
            keybind.SetActive(false);
        }
    }
}
