using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class CameraInstantiator : NetworkBehaviour
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

		if (FindObjectOfType<Camera>() == null)
		{
			camera = Instantiate(ARCameraPrefab);
		}
		else camera = GameObject.FindWithTag("MainCamera");
		if (scene.buildIndex == 0)
		{
			camera.GetComponent<VuforiaBehaviour>().enabled = false;
			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
			FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			if (!isServer)
			{
				camera.GetComponent<VuforiaBehaviour>().enabled = false;
				camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
				FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				camera.GetComponent<VuforiaBehaviour>().enabled = true;
				camera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
				FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(false);
			}
		}
	}
	
}
