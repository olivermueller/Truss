using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

        public InputField ipInput;
        public InputField matchNameInput;

        public Button createButton;
        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

            matchNameInput.onEndEdit.RemoveAllListeners();
            matchNameInput.onEndEdit.AddListener(onEndEditGameName);
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
            lobbyManager.ChangeTo(lobbyPanel);

            lobbyManager.networkAddress = ipInput.text;
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
            
            FindObjectOfType<LobbyPlayer>().gameObject.SetActive(false);
        }

        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        private List<MatchInfoSnapshot> matchesInLobby;
        public void OnClickCreateMatchmakingGame()
        {
            matchNameInput.text = matchNameInput.text.ToUpper();
            createButton.interactable = false;
            if (matchNameInput.text.Length == 0)
            {

                lobbyManager.StartMatchMaker();
                
                //matchesInLobby = new List<MatchInfoSnapshot>();
                lobbyManager.matchMaker.ListMatches(0, 2, "", true, 0, 0, OnGetGames);
                //print("----Matches in Lobby: " + matchesInLobby.Count);
                var f = CreateAfter(5);
                StartCoroutine(f);
            }
            else
            {
                lobbyManager.StartMatchMaker();
                //matchesInLobby = new List<MatchInfoSnapshot>();
                lobbyManager.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnGetGames);
                var f = JoinAfter(5);
                StartCoroutine(f);
            }

        }

        string GetRAndomCode(int from, int to, int size)
        {
            string finalCode = "";
            
            for (int i = 0; i < size; i++)
            {
                finalCode += ((char)(Random.Range(from, to))).ToString();
            }

            return finalCode;
        }

        IEnumerator CreateAfter(float sec)
        {
            lobbyManager.DisplayIsConnectingNoCancel();
            string lobbyName = GetRAndomCode(65, 90, 5);
            yield return new WaitForSeconds(sec);   
            createButton.interactable = true;
            int serverCount = 0;

            for (int i = 0; i < matchesInLobby.Count; i++)
            {
                if (matchesInLobby[i].name == lobbyName + serverCount)
                {
                    serverCount++;
                }
            }
            if(serverCount > 0)
                lobbyName += serverCount.ToString();
            lobbyManager.matchMaker.CreateMatch(
                lobbyName,
                (uint) lobbyManager.maxPlayers,
                true,
                "", "", "", 0, 0,
                lobbyManager.OnMatchCreate);

//            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
            lobbyManager.serverText.text = "USE THE CODE: ";
            lobbyManager.serverCode.text = lobbyName;
            lobbyManager.status.text = "WAITING...";
        }

        IEnumerator JoinAfter(float sec)
        {
            lobbyManager.DisplayIsConnectingNoCancel();
            yield return new WaitForSeconds(sec);
            createButton.interactable = true;
            NetworkID nID = NetworkID.Invalid;
            for (int i = 0; i < matchesInLobby.Count; i++)
            {
                if (matchesInLobby[i].name == matchNameInput.text)
                {
                    nID = matchesInLobby[i].networkId;
                }
            }

            if (nID != NetworkID.Invalid)
            {
                lobbyManager.ChangeTo(lobbyServerList);
                //lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager._isMatchmaking = true;
               //lobbyManager.DisplayIsConnecting();
                lobbyManager.serverText.text = "CONNECTED TO: ";
                lobbyManager.serverCode.text = matchNameInput.text;
                lobbyManager.status.text = "READY!";
                lobbyManager.matchMaker.JoinMatch(nID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
                
            }
            else
            {
                lobbyManager.DisplayInvalidConnection();
                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            }

        }

        public void OnGetGames(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            
            matchesInLobby = matches;
            print("Match Size in Function: " + matchesInLobby.Count);
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }

    }
}
