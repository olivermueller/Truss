using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SingleFocus : MonoBehaviour {

	
	void Update ()
    {
        //if (Input.touchCount == 1)
        {
            //Debug.Log("touched screen");

            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
	}
}
