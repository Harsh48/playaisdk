using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayAI.PlayAIManager playAIManager;
    private PlayAI.ActionBarManager actionBarManager;
    public Button startRecordingButton;
    public Button stopRecordingButton;

    void Start()
    {
        Debug.Log("GameManager Start");
        // Initialize PlayAIManager
        playAIManager = FindObjectOfType<PlayAI.PlayAIManager>();
        if (playAIManager == null)
        {
            Debug.Log("PlayAIManager not found, loading from Resources.");
            GameObject playAIManagerPrefab = Resources.Load<GameObject>("PlayAIManager");
            if (playAIManagerPrefab != null)
            {
                GameObject playAIManagerInstance = Instantiate(playAIManagerPrefab);
                playAIManager = playAIManagerInstance.GetComponent<PlayAI.PlayAIManager>();
                Debug.Log("PlayAIManager instantiated from prefab.");
            }
            else
            {
                Debug.LogError("PlayAIManager prefab not found in Resources.");
            }
        }
        else
        {
            Debug.Log("PlayAIManager found in the scene.");
        }

        // Try to find and initialize ActionBarManager
        actionBarManager = FindObjectOfType<PlayAI.ActionBarManager>();
        if (actionBarManager == null)
        {
            Debug.Log("ActionBarManager not found, loading from Resources.");
            GameObject actionBarManagerPrefab = Resources.Load<GameObject>("ActionBarManager");
            if (actionBarManagerPrefab != null)
            {
                GameObject actionBarManagerInstance = Instantiate(actionBarManagerPrefab);
                actionBarManager = actionBarManagerInstance.GetComponent<PlayAI.ActionBarManager>();
                Debug.Log("ActionBarManager instantiated from prefab.");
            }
            else
            {
                Debug.LogError("ActionBarManager prefab not found in Resources.");
            }
        }
        else
        {
            Debug.Log("ActionBarManager found in the scene.");
        }

        // Ensure both managers are initialized
        if (playAIManager != null && actionBarManager != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found.");
            }
            else
            {
                Debug.Log("Main Camera found.");
            }

            GameObject actionBarPrefab = Resources.Load<GameObject>("ActionBarCanvas");
            if (actionBarPrefab != null)
            {
                playAIManager.Initialize("your_game_id", ".game-container", mainCamera, actionBarPrefab);
                Debug.Log("PlayAIManager initialized.");
            }
            else
            {
                Debug.LogError("ActionBarCanvas prefab not found in Resources.");
            }
        }

        if (startRecordingButton != null)
        {
            startRecordingButton.onClick.AddListener(StartRecording);
            Debug.Log("StartRecordingButton listener added.");
        }
        else
        {
            Debug.LogError("StartRecordingButton is not assigned in the Inspector.");
        }

        if (stopRecordingButton != null)
        {
            stopRecordingButton.onClick.AddListener(StopRecording);
            Debug.Log("StopRecordingButton listener added.");
        }
        else
        {
            Debug.LogError("StopRecordingButton is not assigned in the Inspector.");
        }
    }

    public void AuthenticateUser()
    {
        if (playAIManager != null)
        {
            StartCoroutine(playAIManager.AuthenticateUser("your_api_key", "google", "user@google.com"));
        }
        else
        {
            Debug.LogError("PlayAIManager is not initialized.");
        }
    }

    public void StartRecording()
    {
        if (playAIManager != null)
        {
            playAIManager.StartRecording();
        }
        else
        {
            Debug.LogError("PlayAIManager is not initialized.");
        }
    }

    public void StopRecording()
    {
        if (playAIManager != null)
        {
            playAIManager.StopRecording();
        }
        else
        {
            Debug.LogError("PlayAIManager is not initialized.");
        }
    }

    public void StopStream()
    {
        if (playAIManager != null)
        {
            playAIManager.StopStream();
        }
        else
        {
            Debug.LogError("PlayAIManager is not initialized.");
        }
    }

    public void Logout()
    {
        if (playAIManager != null)
        {
            playAIManager.Logout();
        }
        else
        {
            Debug.LogError("PlayAIManager is not initialized.");
        }
    }
}
