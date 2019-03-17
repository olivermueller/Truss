using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class CameraInstantiator : MonoBehaviour
{

	public GameObject ARCameraPrefab;
	

	private GameObject camera;
	private bool initializedDelegate = false;
	void Awake()
	{
		if (!initializedDelegate)
		{
			initializedDelegate = false;
			SceneManager.sceneLoaded += InitVuforia;
		}

		

	}


	void InitVuforia(Scene scene, LoadSceneMode mode)
	{
		TrackerManager.Instance.GetStateManager().ReassociateTrackables();

		if (GameObject.FindWithTag("MainCamera") == null)
		{
			print("Instatiating");
			camera = Instantiate(ARCameraPrefab);
		}
		else
		{
			camera = GameObject.FindWithTag("MainCamera");
			print(camera.gameObject.name);
		}
		
		//starting scene
		if (scene.buildIndex == 0 || scene.buildIndex == 2)
		{
			camera.GetComponent<VuforiaBehaviour>().enabled = false;
			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
			if(scene.buildIndex == 0) FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(true);
		}
		else
		{
//			if (isServer)
//			{
//				camera.GetComponent<VuforiaBehaviour>().enabled = false;
//				camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
//				FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(false);
//				//XAPIManager.instance.Send("http://adlnet.gov/expapi/verbs/launched", "launched");
//			}
//			else
			{
				camera.GetComponent<VuforiaBehaviour>().enabled = true;
				camera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
				FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(false);
			}
			
		}
		
	}
	
}
