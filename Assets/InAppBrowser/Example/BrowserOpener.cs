using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using System.IO;
using System.Xml;
public class BrowserOpener : MonoBehaviour {

	public string pageToOpen;

    public void Awake()
    {
        var xmlPath = Path.Combine(Application.streamingAssetsPath, "config.xml");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlPath);
        pageToOpen = xmlDocument.DocumentElement.SelectSingleNode("/link").InnerText.ToString();
    }

    // check readme file to find out how to change title, colors etc.
    public void OpenPage() {
		FindObjectOfType<LobbyManager>()?.transform.GetChild(0).gameObject.SetActive(true);
		if (pageToOpen == "")
		{
			GameObject.FindWithTag("EndScreen").transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			GameObject.FindWithTag("EndScreen")?.transform.GetChild(0).gameObject.SetActive(true);
			InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
			options.displayURLAsPageTitle = false;
			options.pageTitle = "Dashboard";

			InAppBrowser.OpenURL(pageToOpen, options);
		}
	
	}

    public void OnClearCacheClicked()
    {
        InAppBrowser.ClearCache();
    }
}
