using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerTaskData : TaskData
{
    public GameObject yesPrefab, noPrefab;
    public GameObject instantiatedYesButton, instantiatedNoButton;
    public TaskData noTask;

    public void SetNoTask(TaskData no)
    {
        noTask = no;
    }

    public TaskData StartNoTask()
    {
        noTask.StartTask();
        return noTask;
    }
    
    private bool? _finished;
    private LineRenderer lr;
    public override void StartTask()
    {
        if(goalPosition) lr = gameObject.AddComponent<LineRenderer>();
        
        _finished = null;
        base.StartTask();
        GameObject canvas = FindObjectOfType<Canvas>().gameObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
        instantiatedYesButton = Instantiate(yesPrefab);
        if(noTask != null)
        {
            instantiatedNoButton = Instantiate(noPrefab);
            instantiatedNoButton.transform.SetParent(canvas.transform);
            instantiatedNoButton.GetComponent<Button>().onClick.AddListener(NoButton);
        }
        else
        {
            instantiatedYesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        }
        instantiatedYesButton.transform.SetParent(canvas.transform);

        instantiatedYesButton.GetComponent<Button>().onClick.AddListener(YesButton);


    }

    public void YesButton()
    {
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = true;
    }
    
    public void NoButton()
    {
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = false;
    }
        public override bool? IsCompleted()
        {
            
            if(goalPosition && lr)Utilities.DrawLine(lr, Camera.main.transform.position - new Vector3(0, 0.01f, 0), goalPosition.position, 0.05f,Color.red);
            if(_finished != null) Destroy(lr);
            return _finished;
        }

}
