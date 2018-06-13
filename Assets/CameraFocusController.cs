
// full tutorial here:
// https://medium.com/@harmittaa/setting-camera-focus-mode-for-vuforia-arcamera-in-unity-6b3745297c3d

using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraFocusController : MonoBehaviour
{

    void Update()
    {
        if (Input.touchCount == 1)
        {
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
        }
    }
}
