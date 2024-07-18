using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine.Networking;

namespace PlayAI
{
    public class PlayAI
    {
        private string gameID;
        private string gameContainer;
        private Camera mainCamera;
        private WebSocket ws;
        private bool isRecording;
        private List<byte[]> frameBuffer;
        private Coroutine recordingCoroutine;

        public PlayAI(string gameID, string gameContainer, Camera camera)
        {
            this.gameID = gameID;
            this.gameContainer = gameContainer;
            this.mainCamera = camera;
            this.frameBuffer = new List<byte[]>();
            InitializeSDK();
        }

        private void InitializeSDK()
        {
            Debug.Log("PlayAI SDK initialized with GameID: " + gameID);
            // ws = new WebSocketServer("wss://api.playai.network/sdk/stream/demo");
             ws = new WebSocket ("ws://connect.websocket.in/v3/1?api_key=VCXCEuvhGcBDP7XhiJJUDvR1e1D3eiVjgZ9VRiaV&notify_self");
             PrintWebSocketDetails(ws);
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket connection opened.");
                PrintWebSocketDetails(ws);
                if (isRecording)
                {
                    Debug.Log("WebSocket reconnected. Resuming frame sending.");
                    SendFrames(); // Attempt to send any buffered frames
                }
            };
            ws.OnClose += (sender, e) =>
            {
                  PrintWebSocketDetails(ws);
                Debug.Log("WebSocket connection closed. Attempting to reconnect...");
                ReconnectWebSocket();
            };
            ws.OnError += (sender, e) => Debug.LogError("WebSocket error: " + e.Message);
            ws.OnMessage += (sender, e) => Debug.Log("WebSocket message received: " + e.Data);
            ConnectWebSocket();
        }

        private void ConnectWebSocket()
        {
            if (ws != null && ws.ReadyState != WebSocketState.Open && ws.ReadyState != WebSocketState.Connecting)
            {
                Debug.Log("Connecting WebSocket...");
                ws.ConnectAsync();
            }
        }

        private void ReconnectWebSocket()
        {
            if (ws != null && ws.ReadyState != WebSocketState.Open && ws.ReadyState != WebSocketState.Connecting)
            {
                Debug.Log("Reconnecting WebSocket...");
                ws.ConnectAsync();
            }
        }

        public IEnumerator StartRecording()
        {
            if (!isRecording)
            {
                isRecording = true;

                // Ensure WebSocket is connected before starting the recording
                while (ws != null && ws.ReadyState != WebSocketState.Open)
                {
                    Debug.Log("Waiting for WebSocket connection...");
                    ws.ConnectAsync();
                     PrintWebSocketDetails(ws);
                    yield return new WaitForSeconds(1);
                    
                }

                recordingCoroutine = CoroutineRunner.StartRoutine(Record());
                Debug.Log("Recording started.");
            }
        }

        public IEnumerator StopRecording()
        {
            if (isRecording)
            {
                isRecording = false;
                CoroutineRunner.StopRoutine(recordingCoroutine);
                Debug.Log("Recording stopped.");
            }
            yield return null;
        }

        private IEnumerator Record()
        {
            while (isRecording)
            {
                yield return new WaitForEndOfFrame();
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();

                byte[] bytes = texture.EncodeToJPG();
                frameBuffer.Add(bytes);

                DestroyUtility.DestroyObject(texture);

                if (frameBuffer.Count > 30) // Adjust the buffer size as needed
                {
                    frameBuffer.RemoveAt(0);
                }

                SendFrames();
            }
        }

        private void SendFrames()
        {
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                foreach (var frame in frameBuffer)
                {
                    ws.Send(frame);
                }
                frameBuffer.Clear();
            }
            else
            {
                Debug.LogError("WebSocket connection is not open. Attempting to reconnect...");
                ReconnectWebSocket();
            }
        }

        public void StopStream()
        {
            if (ws != null && ws.IsAlive)
            {
                ws.Close();
                Debug.Log("Stream stopped.");
            }
        }

        public void LoginWithSessionToken(string sessionToken)
        {
            Debug.Log("Logged in with session token: " + sessionToken);
        }

        public void Logout()
        {
            Debug.Log("Logged out.");
        }

         private void PrintWebSocketDetails(WebSocket ws)
    {
        if (ws != null)
        {
            Debug.Log("WebSocket Details:");
            Debug.Log("URL: " + ws.Url);
            Debug.Log("ReadyState: " + ws.ReadyState);
            Debug.Log("Protocol: " + ws.Protocol);
        }
        else
        {
            Debug.Log("WebSocket is null.");
        }
    }
    }
}
