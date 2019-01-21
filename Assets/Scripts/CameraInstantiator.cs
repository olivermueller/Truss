using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Prototype.NetworkLobby;
using UnityEngine;
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
		print("INITIALIZED VUFORIA============");
		TrackerManager.Instance.GetStateManager().ReassociateTrackables();

		if (FindObjectOfType<Camera>() == null)
		{
			camera = Instantiate(ARCameraPrefab);
		}
		camera = GameObject.FindWithTag("MainCamera");
		if (scene.buildIndex == 0)
		{
			camera.GetComponent<VuforiaBehaviour>().enabled = false;
			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
			GameObject.FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			camera.GetComponent<VuforiaBehaviour>().enabled = true;
			camera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
			//GameObject.FindWithTag("MainPanel").gameObject.SetActive(false);
		}
	}
	
}
