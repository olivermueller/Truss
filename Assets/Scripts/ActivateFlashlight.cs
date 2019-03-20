using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ActivateFlashlight : MonoBehaviour
{
	private bool active = false;
	public void SwitchFlashlight()
	{
		active = !active;
		CameraDevice.Instance.SetFlashTorchMode( active );
	}
}
