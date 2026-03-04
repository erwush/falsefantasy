using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class ChangeScene : MonoBehaviour
{
    public bool inArea;
    public GameObject keybind;
    public string scene;
    public VideoPlayer video;
    public GameObject screen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    IEnumerator PlayVideo(string scene)
    {
        screen.SetActive(true);
        video.Play();
        yield return new WaitForSeconds((float)video.clip.length - 1.7f);
        screen.SetActive(false);
        SceneManager.LoadScene(scene);

    }

    // Update is called once per frame
    void Update()
    {
        if (inArea)
        {
            keybind.SetActive(true);
        }
        else
        {
            keybind.SetActive(false);
        }

        if (inArea && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(PlayVideo(scene));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            inArea = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            inArea = false;
        }
    }


}
