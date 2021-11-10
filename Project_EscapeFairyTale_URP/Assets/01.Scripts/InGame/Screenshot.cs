using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static void TakeScreenshot()
    {
        string path = Application.dataPath;
        ScreenCapture.CaptureScreenshot(Path.Combine(path, "../sc/saved.png"));
    }
}