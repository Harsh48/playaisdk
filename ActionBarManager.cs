using UnityEngine;
using UnityEngine.UI;

namespace PlayAI
{
    public class ActionBarManager : MonoBehaviour
    {
        public GameObject actionBarCanvasPrefab;
        private GameObject actionBarCanvasInstance;

        private Button startRecordingButton;
        private Button stopRecordingButton;
        private Button hideActionBarButton;

        private PlayAIManager playAIManager;

        public void Initialize(GameObject actionBarPrefab, PlayAIManager playAIManager)
        {
            this.playAIManager = playAIManager;

            // Instantiate the ActionBarCanvas prefab
            actionBarCanvasInstance = Instantiate(actionBarPrefab);
            actionBarCanvasInstance.SetActive(false); // Initially hide the action bar

            // Find buttons in the instantiated prefab
            startRecordingButton = actionBarCanvasInstance.transform.Find("ActionBar/StartRecordingButton").GetComponent<Button>();
            stopRecordingButton = actionBarCanvasInstance.transform.Find("ActionBar/StopRecordingButton").GetComponent<Button>();
            hideActionBarButton = actionBarCanvasInstance.transform.Find("ActionBar/HideActionBarButton").GetComponent<Button>();

            // Assign button click listeners
            startRecordingButton.onClick.AddListener(StartRecording);
            stopRecordingButton.onClick.AddListener(StopRecording);
            hideActionBarButton.onClick.AddListener(HideActionBar);
        }

        public void StartRecording()
        {
            playAIManager.StartRecording();
        }

        public void StopRecording()
        {
            playAIManager.StopRecording();
        }


        public void HideActionBar()
        {
            actionBarCanvasInstance.SetActive(false);
        }
    }
}

