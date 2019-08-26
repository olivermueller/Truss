using UnityEngine;
using System.Linq;
using Prototype.NetworkLobby;
using System.IO;
using System.Xml;
public class BrowserOpener : MonoBehaviour {

	public string pageToOpen;

    // check readme file to find out how to change title, colors etc.
    public void OpenPage() {

        //figure out if its a trainer or trainee
        var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);

        var xmlNodeName = "";
        if (player.IsTrainer) xmlNodeName = "/root/TrainerLink";
        else xmlNodeName = "/root/TraineeLink";
        var xmlPath = Path.Combine(Application.streamingAssetsPath, "config.xml");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlPath);

        pageToOpen = xmlDocument.DocumentElement.SelectSingleNode(xmlNodeName).InnerText;
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
