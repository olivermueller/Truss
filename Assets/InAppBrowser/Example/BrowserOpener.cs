using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;

public class BrowserOpener : MonoBehaviour {

	public string pageToOpen = "";

	// check readme file to find out how to change title, colors etc.
	private void Start() {
		GameObject.FindObjectOfType<LobbyManager>().transform.GetChild(0).gameObject.SetActive(true);
		if (pageToOpen == "")
		{
			GameObject.FindWithTag("EndScreen").transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
			options.displayURLAsPageTitle = false;
			options.pageTitle = "Dashboard";

			InAppBrowser.OpenURL(pageToOpen, options);
		}
	
	}

	public void OnClearCacheClicked() {
		InAppBrowser.ClearCache();
	}
}
