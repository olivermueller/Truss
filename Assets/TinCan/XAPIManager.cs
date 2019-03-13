using System;
using System.Collections;
using System.Collections.Generic;
using TinCan;
using TinCan.LRSResponses;
using UnityEngine;

public class XAPIManager : MonoBehaviour
{
    RemoteUnityLRS lrs = new RemoteUnityLRS(
        "https://www.competenceanalytics.com/data/xAPI",
        "281952f03d4e45540fa67e463ffdbada545b2875",
        "515a7b69ceb7de02ae8c96e058973f02d9e297b4"
    );

    public string SessionID = "";    //unique id for the training session that is ran
    public string AgentName = "2017TinCan"; // trainer or trainee
    public string AgentEmail = "BobDylan@example.com";

    private Dictionary<int, Statement> _statementQueue;
	public static XAPIManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
	    _statementQueue = new Dictionary<int, Statement>();
	    DontDestroyOnLoad(gameObject);
	}
	
	public void Send(string VerbID, string VerbAction, string name = null, string activityID = null)
    {
        if (name == null)
        {
            name = AgentName;
        }
        var actor = new Agent();
        actor.name = name;
        actor.mbox = "mailto:" + instance.AgentEmail;

        var verb = new Verb();
        verb.id = new Uri(VerbID);
        verb.display = new LanguageMap();
        verb.display.Add("en-US", VerbAction);

        var activity = new Activity();
        activity.id = activityID ?? "http://activitystrea.ms/schema/1.0/application";
        
        var account = new AgentAccount();
        account.name = instance.SessionID;
        actor.account = account;
        
        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;

        Debug.Log("Sending a message async...");
        StartCoroutine(sendMessageAsync(statement));
        Debug.Log("... message sent");
    }
    
    public void AddToQueue(int QueueID,string VerbID, string VerbAction, string ActivityID)
    {
        var actor = new Agent();
        actor.name = instance.AgentName;
        actor.mbox = "mailto:" + instance.AgentEmail;

        var verb = new Verb();
        verb.id = new Uri(VerbID);
        verb.display = new LanguageMap();
        verb.display.Add("en-US", VerbAction);

        var activity = new Activity();
        activity.id = ActivityID;

        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;

        _statementQueue.Add(QueueID, statement);
    }

    public void RemoveFromQueueAt(int index)
    {
        _statementQueue.Remove(index);
    }
    
    public void SendQueuedStatements()
    {
        foreach (var statement in _statementQueue.Values)
        {
            StartCoroutine(sendMessageAsync(statement));
        }
        _statementQueue.Clear();
    }
    
    void SendMessage(Statement statement)
    {
  
        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);

        if (lrsResponse.success)
        {
           Debug.Log("Save statement: " + lrsResponse.content.id);
        }
        else
        {
            // Do something with failure
        }
    }

    IEnumerator sendMessageAsync(Statement statement)
    {
        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);

        if (lrsResponse.success)
        {
            Debug.Log("Save statement: " + lrsResponse.content.id); // This will not print because we are in a coroutine
        }
        else
        {
            // Do something with failure
        }

        yield return null;
    }

}
