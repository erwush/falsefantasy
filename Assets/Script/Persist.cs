using UnityEngine;

public class Persist : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Persist Instance;

    [Header("Persisntent Object")]
    public GameObject[] persistentObjects;
    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }
    
    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}
