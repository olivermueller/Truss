using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTaskData : TaskData {

    Renderer _renderer;
    public override void StartTask()
    {
        base.StartTask();
        XAPIManager.instance.Send("http://activitystrea.ms/schema/1.0/complete", "completed", "", "http://example.com/application");
        //GameObject.FindGameObjectWithTag("EndScreen").transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LoadSceneAfter(1));
    }

    IEnumerator LoadSceneAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene("[NETWORKED]DashBoard");
    }
    
    public override bool? IsCompleted()
    {
        return null;
    }
}
