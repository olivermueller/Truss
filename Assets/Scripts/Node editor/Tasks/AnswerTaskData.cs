﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public bool? _finished;
    public override void StartTask()
    {
        _finished = true;
        base.StartTask();
        if (goalPosition)
        {
            offscreeArrowScript.Targets.Add(goalPosition.gameObject);
            offscreeArrowScript.Initialize();
        }
        
//        _finished = null;
        
      // GameObject canvas = FindObjectOfType<Canvas>().gameObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
        
//        instantiatedYesButton = Instantiate(yesPrefab);
//        if(noTask != null)
//        {
//            instantiatedNoButton = Instantiate(noPrefab);
//            instantiatedNoButton.transform.SetParent(canvas.transform);
//            instantiatedNoButton.GetComponent<Button>().onClick.AddListener(NoButton);
//        }
//        else
//        {
//            instantiatedYesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
//        }
//        instantiatedYesButton.transform.SetParent(canvas.transform);
//
//        instantiatedYesButton.GetComponent<Button>().onClick.AddListener(YesButton);


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
        
        //if(_finished != null) offscreeArrowScript.Targets.Clear();
        return _finished;
    }

}
