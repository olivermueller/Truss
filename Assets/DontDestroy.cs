using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class DontDestroy : MonoBehaviour {
	private void Awake()
	{
		VuforiaRuntime.Instance.InitVuforia();
	}

	// Use this for initialization
	private void OnDestroy()
	{
		VuforiaRuntime.Instance.Deinit();
	}


}
