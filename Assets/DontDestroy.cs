using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class DontDestroy : MonoBehaviour {
	private void Awake()
	{
		//VuforiaRuntime.Instance.InitVuforia();
		VuforiaBehaviour.Instance.enabled = true;
	}

	// Use this for initialization
	private void OnDestroy()
	{
		VuforiaBehaviour.Instance.enabled = false;

		//VuforiaRuntime.Instance.Deinit();
	}


}
