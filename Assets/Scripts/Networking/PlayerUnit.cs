using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using Prototype.NetworkLobby;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vuforia;

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

    private void Awake()
    {
        VuforiaRuntime.Instance.InitVuforia();
    }

    private void Start()
    {
        
        StartCoroutine(OnReady());

    }

    IEnumerator OnReady()
    {
        if (isServer && isLocalPlayer)
        {
            yield return new WaitUntil(()=>connectionToClient.isReady);
            var camera = FindObjectOfType<Camera>().gameObject;
            camera.GetComponent<VuforiaBehaviour>().enabled = true;
            camera.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
            FindObjectOfType<LobbyManager>().transform.GetChild(1).gameObject.SetActive(false);
//            _nextButton = Instantiate(NextButtonPrefab).GetComponent<Button>();
//            _nextButton.transform.parent = FindObjectOfType<Canvas>().transform;
//            _nextButton.onClick.AddListener(() => CmdOnClickSetAwating(true));
            RpcTellAllClientsToUpdateRoles(true);
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.5f);
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
        XAPIManager.instance.SendQueuedStatements();
    }
    
    [Command]
    public void CmdTrainerNotApproved()
    {
        GameStateManager = GameStateManager == null ? FindObjectOfType<NetworkedGameState>() : GameStateManager;

        
//        GameStateManager.CmdSetNodeId((FindObjectOfType<TestingScript>().iterator as AnswerTaskData).noTask.ID);
        //GameStateManager._testingScript= GetComponent<TestingScript>();

        AnswerTaskData answerTask = GameStateManager._testingScript.iterator as AnswerTaskData;
        AnswerTargetTaskData answerTargetTask = GameStateManager._testingScript.iterator as AnswerTargetTaskData;
        NestedTaskData nestedTask = GameStateManager._testingScript.iterator as NestedTaskData;


        if (answerTask)
        {
            if(answerTask.noTask != null)
                GameStateManager.CmdSetNodeId(answerTask.noTask.ID);
            else
                GameStateManager.CmdSetNodeId(answerTask.ID);

        }
        else if (answerTargetTask)
        {
            if(answerTargetTask.noTask != null)
                GameStateManager.CmdSetNodeId(answerTargetTask.noTask.ID);
            else
                GameStateManager.CmdSetNodeId(answerTargetTask.ID);
        }
        
        GameStateManager.CmdSetDenied(true);
        GameStateManager.CmdSetAwating(false);
        GameStateManager.CmdSetApproved(false);
       // GameStateManager._testingScript.iterator.StartTask();

       
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
        var iterator = FindObjectOfType<TestingScript>().iterator;

        XAPIManager.instance.Send("http://adlnet.gov/expapi/verbs/attempted", "attempted", "Trainee", "http://example.com/node/" + iterator.XapiID);

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
        GameStateManager.CmdSetDenied(false);
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


    void Update()
    {
        //Update UI on the trainer side
        if (IsTrainer && isServer && isLocalPlayer)
        {
            trainerUI.SetActive(true);
        }
        else if (!IsTrainer && !isServer && isLocalPlayer)
        {
            traineeUI.SetActive(true);
        }
    }

    [ClientRpc] //fn executed on all clients
    void RpcTellAllClientsToUpdateRoles(bool newValue)
    {
        print("Called UpdateRoles");
        IsTrainer = newValue;
//        if(IsTrainer)
//            trainerUI.SetActive(true);
//        else
//            traineeUI.SetActive(true);
    }

    

}