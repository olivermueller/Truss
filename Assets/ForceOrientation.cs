using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ForceOrientation : MonoBehaviour
{
	private bool first = true;

	void ChangeFocusTo(CameraDevice.FocusMode focusType)
	{
		CameraDevice.Instance.SetFocusMode(focusType);
	}
	
	void LateUpdate()
	{
		if(Screen.orientation != ScreenOrientation.Portrait)
			Screen.orientation = ScreenOrientation.Portrait;	
		
	}

//	private void OnGUI()
//	{
//		if (GUI.Button(new Rect(10, 0, 150, 150), "FOCUS_MODE_CONTINUOUSAUTO"))
//		{
//			ChangeFocusTo(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
//		}
//		if (GUI.Button(new Rect(10, 150, 150, 150), "FOCUS_MODE_INFINITY"))
//		{
//			ChangeFocusTo(CameraDevice.FocusMode.FOCUS_MODE_INFINITY);
//		}
//		if (GUI.Button(new Rect(10, 300, 150, 150), "FOCUS_MODE_MACRO"))
//		{
//			ChangeFocusTo(CameraDevice.FocusMode.FOCUS_MODE_MACRO);
//		}
//		if (GUI.Button(new Rect(10, 450, 150, 150), "FOCUS_MODE_NORMAL"))
//		{
//			ChangeFocusTo(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
//		}
//		if (GUI.Button(new Rect(10, 600, 150, 150), "FOCUS_MODE_TRIGGERAUTO"))
//		{
//			ChangeFocusTo(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
//		}
//	}
}
