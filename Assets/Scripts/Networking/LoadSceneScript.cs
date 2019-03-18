using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class LoadSceneScript : MonoBehaviour {

	public void LoadScene(int sceneToLoad)
	{
		NetworkClient.ShutdownAll();
		NetworkServer.DisconnectAll();
		StartCoroutine(StopHostAfter(2));
	}

	IEnumerator StopHostAfter(int t)
	{
		yield return new WaitForSeconds(t);
		
		SceneManager.LoadScene(0);
	}
	
	public void LoadSceneTest(int sceneToLoad)
	{
		SceneManager.LoadScene(sceneToLoad);
	}

	

//	private void OnEnable()
//	{
//		VuforiaBehaviour.Instance.enabled = true;
//	}
//	
//	private void OnDisable()
//	{
//		VuforiaBehaviour.Instance.enabled = false;
//	}
}
