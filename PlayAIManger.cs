using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading.Tasks;

namespace PlayAI
{
    public class PlayAIManager : MonoBehaviour
    {
        private PlayAI playAI;
        private ActionBarManager actionBarManager;

        public void Initialize(string gameID, string gameContainer, Camera camera, GameObject actionBarPrefab)
        {
            Debug.Log("Initializing PlayAIManager");
            playAI = new PlayAI(gameID, gameContainer, camera);
            actionBarManager = FindObjectOfType<ActionBarManager>();
            if (actionBarManager != null)
            {
                actionBarManager.Initialize(actionBarPrefab, this);
            }
            else
            {
                Debug.LogError("ActionBarManager not found in the scene.");
            }
        }

        public void HideActionBar()
        {
            if (actionBarManager != null)
            {
                actionBarManager.HideActionBar();
            }
            else
            {
                Debug.LogError("ActionBarManager is not initialized.");
            }
        }

        public void StartRecording()
        {
            if (playAI != null)
            {
                Debug.Log("Starting recording");
                StartCoroutine(playAI.StartRecording());
            }
            else
            {
                Debug.LogError("PlayAI is not initialized.");
            }
        }

        public void StopRecording()
        {
            if (playAI != null)
            {
                Debug.Log("Stopping recording");
                StartCoroutine(playAI.StopRecording());
            }
            else
            {
                Debug.LogError("PlayAI is not initialized.");
            }
        }

        public void StopStream()
        {
            if (playAI != null)
            {
                playAI.StopStream();
            }
            else
            {
                Debug.LogError("PlayAI is not initialized.");
            }
        }

        public void Logout()
        {
            if (playAI != null)
            {
                playAI.Logout();
            }
            else
            {
                Debug.LogError("PlayAI is not initialized.");
            }
        }

        public IEnumerator AuthenticateUser(string apiKey, string type, string account)
        {
            string url = "https://api.playai.network/sdk/your_game_id/session/create";

            var request = new UnityWebRequest(url, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-playai-token", apiKey);

            var json = JsonUtility.ToJson(new { type = type, account = account });
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var response = JsonUtility.FromJson<SessionResponse>(request.downloadHandler.text);
                if (playAI != null)
                {
                    playAI.LoginWithSessionToken(response.sessionToken);
                }
                else
                {
                    Debug.LogError("PlayAI is not initialized.");
                }
            }
        }

        [System.Serializable]
        public class SessionResponse
        {
            public string sessionToken;
        }
    }
}
