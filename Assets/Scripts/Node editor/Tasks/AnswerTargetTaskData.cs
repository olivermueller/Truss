using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vuforia;

public class AnswerTargetTaskData : TaskData
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
    
    public bool? _finished;
    
    public override void StartTask()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.AddListener(IsTargetActive);
        _finished = null;
        base.StartTask();
        //CreateButtons(true);
        if (goalPosition)
        {
            offscreeArrowScript.Targets.Add(goalPosition.gameObject);
            offscreeArrowScript.Initialize();
        }
        
        
        

    }

    public void CreateButtons(bool active)
    {
        if (active)
        {
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
        else
        {
            Destroy(instantiatedNoButton);
            Destroy(instantiatedYesButton);
        }
        
    }

    public void YesButton()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.RemoveListener(IsTargetActive);
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = true;
    }
    
    public void NoButton()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.RemoveListener(IsTargetActive);
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = false;
    }
    public override bool? IsCompleted()
    {   
        return _finished;
    }

    public void IsTargetActive(bool active)
    {
        if (active)
            _finished = true;
        else
            _finished = null;

    }

    //USE CREATEBUTTONS FOR A UI POPUP MAYBE
//        targetActive = active;
//        Debug.LogWarning("Callback made on " + _title + "Value: " + active);
//        CreateButtons(active);
    }

