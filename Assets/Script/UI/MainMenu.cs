using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToScene(string scene)
    {
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
