using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public VideoPlayer video;
    public GameObject screen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToScene(string scene)
    {
        StartCoroutine(PlayVideo(scene));
    }

    IEnumerator PlayVideo(string scene)
    {
        screen.SetActive(true);
        video.Play();
        yield return new WaitForSeconds((float)video.clip.length-1.7f);
        screen.SetActive(false);
        SceneManager.LoadScene(scene);

    }
    // (opsional tapi kepake) Pindah scene pakai build index
    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Keluar dari game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // biar keliatan pas di Editor
    }
}
