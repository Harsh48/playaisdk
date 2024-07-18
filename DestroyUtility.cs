using UnityEngine;

public class DestroyUtility : MonoBehaviour
{
    private static DestroyUtility instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void DestroyObject(Object obj)
    {
        if (instance != null)
        {
            Destroy(obj);
        }
    }
}
