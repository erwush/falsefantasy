using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI[] text;
    //0 = DIALOGUE TEXT
    //1 = Name 1
    public Image image;
    public GameObject UI;
    public GameObject obj;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeDialog()
    {
        Dialogable dial = obj.GetComponent<Interactable>().dialogable;

        int i = dial.currentDial;
        if (i >= dial.dialogCount)
        {
            dial.currentDial = 0;
            player.GetComponent<Movement>().canMove = true;
            UI.SetActive(false);
        }
        else
        {
            text[0].text = dial.text[i];
            text[1].text = dial.nama[i];
            image.sprite = dial.avatar[i];
            image.preserveAspect = true;
            dial.currentDial++;
            i++;
            if (i > dial.dialogCount)
            {
                UI.SetActive(false);
                player.GetComponent<Movement>().canMove = true;
            }

        }
    }
}
