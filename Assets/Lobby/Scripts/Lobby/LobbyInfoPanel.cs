using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Prototype.NetworkLobby 
{
    public class LobbyInfoPanel : MonoBehaviour
    {
        public Text infoText;
        public Text buttonText;
        public Button singleButton;

        public void Display(string info, string buttonInfo, UnityEngine.Events.UnityAction buttonClbk, bool showCancel = true)
        {
            infoText.text = info;

            buttonText.text = buttonInfo;

            singleButton.onClick.RemoveAllListeners();

            if (buttonClbk != null)
            {
                singleButton.onClick.AddListener(buttonClbk);
            }

            singleButton.onClick.AddListener(() => { gameObject.SetActive(false); });
            singleButton.gameObject.SetActive(showCancel);

            
            gameObject.SetActive(true);
        }
    }
}