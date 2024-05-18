using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ScreenShot : MonoBehaviour
{
    public string Name;

    [Button("ÂûÄ¬!")]
    public void TakeAShot()
    {
        ScreenCapture.CaptureScreenshot(Name + ".png");
    }
}
