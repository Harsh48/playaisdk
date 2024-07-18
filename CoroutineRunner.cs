using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

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

    public static Coroutine StartRoutine(IEnumerator routine)
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("CoroutineRunner");
            instance = obj.AddComponent<CoroutineRunner>();
        }
        return instance.StartCoroutine(routine);
    }

    public static void StopRoutine(Coroutine routine)
    {
        if (instance != null)
        {
            instance.StopCoroutine(routine);
        }
    }
}
