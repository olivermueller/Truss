using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using TMPro;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class TaskData : MonoBehaviour
{
    public string _title, _description;
    public TaskData _out, _in;
    public GameObject _animationObject, _baseObject, _instantiatedAnimationObject, _uiObject;
    //position of the goal you are being guided towards
    public Transform goalPosition;
    public void SetNextTask(TaskData next)
    {
        _out = next;
    }
    
    public virtual void StartTask()
    {
        Debug.Log("<color=red>Started " + _title+"</color>" );
                     Debug.Log("<color=yellow>Mission: " + _description+"</color>" );
         //        Debug.Log("Started" + _title);
        if (_animationObject != null)
        {
            _instantiatedAnimationObject = GameObject.Instantiate(_animationObject);
            _instantiatedAnimationObject.transform.parent = _baseObject.transform;
        }
        
        GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
        GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
        //
    }
    public virtual TaskData NextTask()
    {
        if (_instantiatedAnimationObject) GameObject.Destroy(_instantiatedAnimationObject);
        Debug.Log("<color=green>completed " + _title+"</color>" );
        if (_out != null)
        {
            _out.StartTask();
            Debug.Log("<color=purple>Next Task " + _out._title+"</color>" );
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
