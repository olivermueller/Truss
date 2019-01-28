using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTaskData : TaskData {

    Renderer _renderer;
    public override void StartTask()
    {
        base.StartTask();
        GameObject.FindGameObjectWithTag("EndScreen").transform.GetChild(0).gameObject.SetActive(true);

    }

    public override bool? IsCompleted()
    {
        return null;
    }
}
