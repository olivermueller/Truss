using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Vuforia;

public delegate void Action();
[System.Serializable]
public class TaskData : NetworkBehaviour
{
    public int XapiID;
    public string ID;
    public string _title, _description;
    public TaskData _out, _in;
    public GameObject _animationObject, _baseObject, _instantiatedAnimationObject, _uiObject;
    public List<String> tasks;
    public TaskData parentTask;
    
    public UnityEvent NodeIdEvent;
    //position of the goal you are being guided towards
    public Transform goalPosition;
    public void SetNextTask(TaskData next)
    {
        _out = next;
    }


    public virtual void StartTask()
    {
        bool excludeStatements = this as StartTaskData || this as FinishTaskData;
        
        
        if (isServer && !excludeStatements)
        {
            XAPIManager.instance.Send("http://adlnet.gov/expapi/verbs/launched", "launched", "Trainee", "http://example.com/node/" + XapiID);
            XAPIManager.instance.Send("http://id.tincanapi.com/verb/viewed", "viewed", "Trainee", "http://example.com/node/" + XapiID);
        }
        GameObject[] checkListElements = GameObject.FindGameObjectsWithTag("CheckListElement");
        if (checkListElements != null)
        {
            for (int i = 0; i < checkListElements.Length; i++)
            {
                Destroy(checkListElements[i]);
            }
        }
        
        
        Debug.Log("<color=red>Started " + _title+"</color>" );
                     Debug.Log("<color=yellow>Mission: " + _description+"</color>" );
         //        Debug.Log("Started" + _title);
        if (_animationObject != null && _instantiatedAnimationObject == null)
        {
            _instantiatedAnimationObject = Instantiate(_animationObject);
            var localscale = _instantiatedAnimationObject.transform.localScale;
            if (_baseObject)
            {
                _instantiatedAnimationObject.transform.parent = _baseObject.transform;
                _instantiatedAnimationObject.transform.localPosition = Vector3.zero;
                _instantiatedAnimationObject.transform.localScale = localscale;
                _instantiatedAnimationObject.transform.localRotation = Quaternion.identity; 
            }
            //check if the previous target is the same as the current one

            if (_in._baseObject != null && _baseObject!=null && _in._baseObject == _baseObject)
            {
                var currentStatus = _instantiatedAnimationObject.transform.parent
                    .GetComponent<MissionTrackableEventHandler>().markerActive;
                print("--------------------" + _instantiatedAnimationObject.name);
                _instantiatedAnimationObject.transform.parent.GetComponent<MissionTrackableEventHandler>()
                    .OnTrackableStateChange.Invoke(currentStatus);
                if (currentStatus)
                    OnTrackingFound(_instantiatedAnimationObject.transform.parent.gameObject);
                else OnTrackingLost(_instantiatedAnimationObject.transform.parent.gameObject);
            }
            else OnTrackingLost(_instantiatedAnimationObject.transform.parent.gameObject);
            
            
            
         
        }
        
        
        
        GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
        GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
        var checkListObj = GameObject.FindGameObjectWithTag("CanvasCheckList");
        if(isServer && !excludeStatements)            XAPIManager.instance.Send("http://www.tincanapi.co.uk/verbs/evaluated", "evaluated", "Trainer", "http://example.com/node/" + XapiID);

        for (int i = 0; i < tasks.Count && checkListObj; i++)
        {
            

            var listElement = Instantiate(TaskModel.Instance.checkListItemPrefab);
            listElement.transform.parent = checkListObj.transform;
            listElement.GetComponentInChildren<TextMeshProUGUI>().text = tasks[i];
            
            if(isServer && !excludeStatements) XAPIManager.instance.Send("http://activitystrea.ms/schema/1.0/rejectzz", "rejected", "Trainer", "http://example.com/node/" + XapiID + "/" + "checklistitem/" + i);
        }
        
    }
    
    void OnTrackingLost(GameObject obj)
    {
        var rendererComponents = obj.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = obj.GetComponentsInChildren<Collider>(true);
        var canvasComponents = obj.GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }
    void OnTrackingFound(GameObject obj)
    {
        var rendererComponents = obj.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = obj.GetComponentsInChildren<Collider>(true);
        var canvasComponents = obj.GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }
    
    public virtual TaskData NextTask()
    {
        
        
        if (_instantiatedAnimationObject) GameObject.Destroy(_instantiatedAnimationObject);
        Debug.Log("<color=green>completed " + _title+"</color>" );
        if (_out != null)
        { 
            Debug.Log("<color=purple>Next Task " + _out._title+"</color>" );
            _out.StartTask();
        }
        return _out;
    }

    public void Initialize(string title, string description, GameObject animationObject, GameObject imageTargetObject)
    {
        _title = title;
        _description = description;
        _animationObject = animationObject;
        _baseObject = imageTargetObject;
    }

    public virtual bool? IsCompleted()
    {
        return false;
    }

}

//    public class TriggerClass : TaskData
//    {
//        Renderer _renderer;
//        public override void Start()
//        {
//            base.Start();
//            _renderer = _baseObject.GetComponent<Renderer>();
//        }
//        public TriggerClass(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
//        {
//        
//        }
//
//        public override bool? IsCompleted()
//        {
//            return IsTriggerEntered();
//        }
//        bool IsTriggerEntered()
//        {
//            return _renderer.enabled;
//        }
//    }
//    public class SubTaskDataTaskData : TaskData
//    {
//        bool isFirst = true;
//        private int subtaskId = 0;
//        public TaskData iterator;
//        public List<TaskData> _subTasks;
//        public override void Start()
//        {
//            iterator = _subTasks[subtaskId];
//            base.Start();
//        }
//
//        public SubTaskDataTaskData(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
//        {
//            _subTasks = new List<TaskData>();
//        }
//
//        public void AddSubTask(TaskData subtask)
//        {
//            _subTasks.Add(subtask);
//        }
//
//        public override bool? IsCompleted()
//        {
//            // First time IsCompleted is called on SubTaskTask, set its local iterator to the first subtask.
//            if(isFirst)
//            {
//                iterator = _subTasks[subtaskId];
//                iterator.Start();
//                isFirst = false;
//            }
//            // Evaluate if the subtask is completed
//            var val = iterator.IsCompleted();
//            if (val.HasValue && val.Value)
//            {
//                // If it is complete, move to the next task
//                iterator = iterator.NextTask();
//                if (iterator == null)
//                {
//                    Debug.Log("Subtask"+ subtaskId +" Completed");
//                    // If there are no more tasks, move to the next subtask if there is any left
//                    if (subtaskId < _subTasks.Count-1)
//                    {
//                        subtaskId++;
//                        iterator = _subTasks[subtaskId];
//                        iterator.Start();
//                    }
//                    // No more subtasks, subtasktask completed
//                    else return true;
//                }
//            }
//            // Subtask is not completed, return false.
//            return false;
//        }
//    }
//    public class YesNoTaskData : TaskData
//    {
//        //state = -1 -> no
//        //state = 0 -> neutral
//        //state = 1 -> true
//
//        public int _state = 0;
//        public override void Start()
//        {
//            base.Start();
//        }
//
//        public override bool? IsCompleted()
//        {
//            switch (_state)
//            {
//                case -1:
//                    return false;
//                case 0:
//                    return null;
//                case 1:
//                    return true;
//            }
//            return null;
//        }
//
//        public void SetState(int state)
//        {
//            _state = state;
//        }
//
//        public YesNoTaskData(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
//        {
//
//        }
//    }
