using System;
using System.Collections;
using TinCan;
using TinCan.LRSResponses;
using UnityEngine;

public class TestTinCan : MonoBehaviour
{

    /**
     * Make an instance of RemoteUnityLRS. This is the custom class adapted from RemoteLRS. It uses the Unity Web Requests.
     */ 
    RemoteUnityLRS lrs = new RemoteUnityLRS(
    "https://www.competenceanalytics.com/data/xAPI",
    "15303f0a40d83d8c0b1c7254e8ff87fa99933360",
    "b14bb572934d8b36cd748868639fdce135a41bf1"
    );
    
    void Start()
    {

        /*
         * You can now use the TinCan API almost exactly as detailed by the creators....
         * See: http://rusticisoftware.github.io/TinCan.NET/ 
         */
        var actor = new Agent();
        actor.name = "2017TinCan";
        actor.mbox = "mailto:BobDylan@example.com";

        var verb = new Verb();
        verb.id = new Uri("https://w3id.org/xapi/adl/verbs/logged-in");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "logged-in");

        var activity = new Activity();
        activity.id = "http://activitystrea.ms/schema/1.0/application";

        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;

        /* This is how you send a message sync. 
         * Only use this way if sure you are not going to cause the application to lag.
         * I would use this function in cases where the application is loading, when getting user data and customising a level with it etc etc
         */
        Debug.Log("Sending a message sync...");
        SendMessage(statement);
        Debug.Log("... message sent");

        statement.actor.name = "2017TinCanAsync";

        /*
         * This is a way to do messaging async. It works... but could be better.
         * Orignally I was going to create a "lrs.SaveStatementAsync(statement)" which could be called without the StartCoroutine function. Callback proved difficult. Can implement if needed.
         */
        Debug.Log("Sending a message async...");
        StartCoroutine(sendMessageAsync(statement));
        Debug.Log("... message sent");
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnApplicationQuit()
    {
        var actor = new Agent();
        actor.name = "2017TinCan";
        actor.mbox = "mailto:secretan@brookes.ac.uk";

        var verb = new Verb();
        verb.id = new Uri("https://w3id.org/xapi/adl/verbs/logged-out");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "logged-out");

        var activity = new Activity();
        activity.id = "http://activitystrea.ms/schema/1.0/application";

        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;

        Debug.Log("Sending a message sync...");
        SendMessage(statement);
        Debug.Log("... message sent");

        statement.actor.name = "2017TinCanAsync";

        Debug.Log("Sending a message async...");
        StartCoroutine(sendMessageAsync(statement));
        Debug.Log("... message sent");
    }
}
