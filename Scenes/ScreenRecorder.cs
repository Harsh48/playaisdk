using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class ScreenRecorder : MonoBehaviour
{
    public int frameRate = 30;
    public string outputFolder = "ScreenCaptures";
    private int frameCount;
    private bool isRecording = false;

    void Start()
    {
        // Set the playback framerate (real time will not relate to game time)
        Time.captureFramerate = frameRate;
        // Create the folder
        Directory.CreateDirectory(outputFolder);
    }

    void Update()
    {
        if (isRecording)
        {
            string name = string.Format("{0}/frame{1:D04}.png", outputFolder, frameCount);
            ScreenCapture.CaptureScreenshot(name);
            frameCount++;
        }

        // Toggle recording with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRecording = !isRecording;
            if (isRecording)
            {
                frameCount = 0;
                UnityEngine.Debug.Log("Recording Started");
            }
            else
            {
                UnityEngine.Debug.Log("Recording Stopped");
                StartCoroutine(ConvertToVideo());
            }
        }
    }

    IEnumerator ConvertToVideo()
    {
        yield return new WaitForEndOfFrame();

        // Path to ffmpeg.exe
        string ffmpegPath = "path/to/ffmpeg.exe";
        // FFmpeg arguments
        string args = string.Format("-r {0} -i {1}/frame%04d.png -c:v libx264 -pix_fmt yuv420p {2}/output.mp4", 
            frameRate, outputFolder, outputFolder);

        // Start ffmpeg process
        ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegPath, args)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        Process process = new Process() { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();

        UnityEngine.Debug.Log("Video Conversion Completed");
    }
}
