using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;
using UnityEngine.UI;

// A PlayerUnit is a unit controlled by a player
// This could be a character in an FPS, a zergling in a RTS
// Or a scout in a TBS

public class PlayerUnit : NetworkBehaviour
{
    public GameObject GameStateManagerPrefab;
    public NetworkedGameState GameStateManager;

    public GameObject trainerUI, traineeUI;
    //public GameObject NextButtonPrefab;

    private Button _nextButton;

    [SyncVar] public bool IsTrainer = false;

    private void Start()
    {
        
        StartCoroutine(OnReady());

    }

    IEnumerator OnReady()
    {
        if (isServer && isLocalPlayer)
        {
            yield return new WaitUntil(()=>connectionToClient.isReady);
            
//            _nextButton = Instantiate(NextButtonPrefab).GetComponent<Button>();
//            _nextButton.transform.parent = FindObjectOfType<Canvas>().transform;
//            _nextButton.onClick.AddListener(() => CmdOnClickSetAwating(true));
            RpcTellAllClientsToUpdateRoles(true);
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(4);
        CmdSpawnMyUnit();
    }

    public void SetNode(string val)
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetNodeId(val);
    }

    public void TraineeNext()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        if (isServer)
        {
            //rpc
            GameStateManager.RpcUITraineeNext();
            
        }
        else
        {
            //cmd
            GameStateManager.CmdUITraineeNext();
        }
    }
    

    
    [Command]
    public void CmdTrainerApproved()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetAwating(true);
        GameStateManager.CmdSetApproved(true);
    }
    [Command]
    public void CmdSetId(string val)
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetNodeId(val);
    }
    
    [Command]
    public void CmdTraineeNext()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetAwating(true);
        GameStateManager.CmdSetApproved(false);
    }
    
    [Command]
    public void CmdResetBools()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetApproved(false);
        GameStateManager.CmdSetAwating(false);
    }
    
    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(GameStateManagerPrefab);
        //go.GetComponent<NetworkIdentity>().AssignClientAuthority( connectionToClient );
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        print("Spawned with authority");

    }

    public void TraineeNextButton()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetApproved(false);
        GameStateManager.CmdSetAwating(true);
    }

    public void TrainerApprovedButton()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;
        GameStateManager.CmdSetApproved(true);
        GameStateManager.CmdSetAwating(true);
    }


//    void Update()
//    {
//        //Update UI on the trainer side
//        if (IsTrainer)
//        {
//            //if (gameStateManager.)
//        }
//        else
//        {
//
//        }
//    }

    [ClientRpc] //fn executed on all clients
    void RpcTellAllClientsToUpdateRoles(bool newValue)
    {
        print("Called UpdateRoles");
        IsTrainer = newValue;
        if(IsTrainer)
            trainerUI.SetActive(true);
        else
            traineeUI.SetActive(true);
    }
//    [SyncVar]
//    public bool Interactable;
//    public Button clientHudDisplay;
//    [SyncVar]
//    public bool isTrainer;
//
//    public PlayerDisplayContainer player1Display, player2Display;
//    void Start(){
//
//
//        
//       // if (isServer)
//       // {
//          //  RpcTellAllClientsToUpdateRoles(isLocalPlayer);
////            RpcTellAllClientsToUpdateHuds(isLocalPlayer);
//       // }
//        
//    }

//    void UpdateRoles()
//    {
//        if (isTrainer)
//        {
//            player1Display.gameObject.SetActive(true);
//        }
//        else
//        {
//            player2Display.gameObject.SetActive(true);
//        }
//    }

//    public void NextButtonClick()
//    {
//        CmdUpdateServerSyncedVariable();
//    }
//
//    public void SwitchRolesButtonClick()
//    {
//        CmdSwitchRoles();
//    }

//    [Command] //fn executed only on server
//    void CmdSwitchRoles()
//    {
//        foreach (var playerUnit in FindObjectsOfType<PlayerUnit>())
//        {
//            playerUnit.isTrainer = !playerUnit.isTrainer;
//            playerUnit.RpcTellAllClientsToUpdateRoles(playerUnit.isTrainer);
//        }
//
//        CmdUpdateServerSyncedVariable();
//    }
//
//    [Command] //fn executed only on server
//    void CmdUpdateServerSyncedVariable(){
//     
//        foreach (var playerUnit in FindObjectsOfType<PlayerUnit>())
//        {
//            playerUnit.Interactable = !playerUnit.Interactable;
////            playerUnit.RpcTellAllClientsToUpdateHuds(playerUnit.Interactable);
//        }

    //It's important to pass the correct updated value as a parameter in your Rpc function!
//        RpcTellAllClientsToUpdateHuds(newValue); //or ...(serverSyncedVariable);

    // }

//    [ClientRpc] //fn executed on all clients
//    void RpcTellAllClientsToUpdateHuds(bool newValue){
//        Interactable = newValue;
//        //this syncs accurately
//        if (isTrainer)
//        {
//            player1Display.nextButton.interactable = newValue;
//        }
//        else
//        {
//            player2Display.nextButton.interactable = newValue;
//        }
//
//    }



//    [ClientRpc] //fn executed on all clients
//    void RpcTellAllClientsToUpdateRoles(bool newValue)
//    {
//        
//        isTrainer = newValue;
//        if (isLocalPlayer)
//        {
//            if (isTrainer)
//            {
//                player1Display.gameObject.SetActive(true);
//                player2Display.gameObject.SetActive(false);
//            }
//            else
//            {
//                player2Display.gameObject.SetActive(true);
//                player1Display.gameObject.SetActive(false);
//
//            }   
//        }
//        
//
//    }

}