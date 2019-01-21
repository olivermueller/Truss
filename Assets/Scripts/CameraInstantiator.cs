using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class CameraInstantiator : MonoBehaviour
{

	public GameObject ARCameraPrefab;
	
	void Awake () 
	{
		
		if (FindObjectOfType<Camera>() == null)
		{
			GameObject.Instantiate(ARCameraPrefab);
		}
//		var camera = GameObject.FindObjectOfType<Camera>().gameObject;
//		if (SceneManager.GetActiveScene().buildIndex == 0)
//		{
//			camera.GetComponent<VuforiaBehaviour>().enabled = false;
//			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
//		}
//		else
//		{
//			camera.GetComponent<VuforiaBehaviour>().enabled = true;
//			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
//		}
			
	}
}
