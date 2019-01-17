using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class DontDestroy : MonoBehaviour {
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelWasLoaded;
	}

	void OnLevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == 1)
		{
			GetComponent<VuforiaBehaviour>().enabled = true;
			GetComponent<DefaultInitializationErrorHandler>().enabled = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			GetComponent<VuforiaBehaviour>().enabled = false;
			GetComponent<DefaultInitializationErrorHandler>().enabled = false;
		}
	}

	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
