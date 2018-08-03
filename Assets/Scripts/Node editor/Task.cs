using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[System.Serializable]
public class Task:MonoBehaviour
{
    public string _title, _description;
    public int ID, nextID;
    [System.NonSerialized]
    protected Task _next;
    public GameObject _animationObject, _baseObject, _instantiatedAnimationObject;

    public void SetNextTask(Task next)
    {
        _next = next;
        nextID = _next.ID;
    }
    public virtual void Start()
    {
        Debug.Log("<color=red>Started " + _title+"</color>" );
        Debug.Log("<color=yellow>Mission: " + _description+"</color>" );
//        Debug.Log("Started" + _title);
        if (_animationObject != null)
        {
            _instantiatedAnimationObject = GameObject.Instantiate(_animationObject);
            _instantiatedAnimationObject.transform.parent = _baseObject.transform;

        }
    }
    public virtual Task NextTask()
    {
        if (_instantiatedAnimationObject) GameObject.Destroy(_instantiatedAnimationObject);
        if(_next!=null) _next.Start();
        Debug.Log("<color=green>completed " + _title+"</color>" );
        Debug.Log("<color=purple>Next Task " + _next._title+"</color>" );
        return _next;
    }

    public Task(string title, string description, GameObject animationObject, GameObject imageTargetObject, int id)
    {
        _title = title;
        _description = description;
        _animationObject = animationObject;
        _baseObject = imageTargetObject;
        ID = id;
    }

    public virtual bool? IsCompleted()
    {
        return false;
    }

}
[System.Serializable]
public class TargetTask:Task
{
    
    Renderer _renderer;
    public override void Start()
    {
        Debug.Log("<color=green> Started "+ _title +"</color>");
        base.Start();
        _renderer = _baseObject.GetComponentInChildren(typeof(Renderer), true) as Renderer;
    }

    public override bool? IsCompleted()
    {
        Debug.Log("checking is completed" + _title);

        return IsImageTargetActive();
    }

    public TargetTask(string title, string description, GameObject animationObject, GameObject imageTargetObject, int id) : base(title, description, animationObject, imageTargetObject, id)
    {
        
    }
    bool IsImageTargetActive()
    {
        return _renderer.enabled;
    }

}
[System.Serializable]
public class TriggerClass : Task
{
    Renderer _renderer;
    public override void Start()
    {
        base.Start();
        _renderer = _baseObject.GetComponent<Renderer>();
    }
    public TriggerClass(string title, string description, GameObject animationObject, GameObject imageTargetObject, int id) : base(title, description, animationObject, imageTargetObject, id)
    {
        
    }

    public override bool? IsCompleted()
    {
        return IsTriggerEntered();
    }
    bool IsTriggerEntered()
    {
        return _renderer.enabled;
    }
}
[System.Serializable]
public class SubTaskTask : Task
{
    bool isFirst = true;
    private int subtaskId = 0;
    public Task iterator;
    public List<Task> _subTasks;
    public override void Start()
    {
        iterator = _subTasks[subtaskId];
        base.Start();
    }

    public SubTaskTask(string title, string description, GameObject animationObject, GameObject imageTargetObject, int id) : base(title, description, animationObject, imageTargetObject, id)
    {
        _subTasks = new List<Task>();
    }

    public void AddSubTask(Task subtask)
    {
        _subTasks.Add(subtask);
    }

    public override bool? IsCompleted()
    {
        // First time IsCompleted is called on SubTaskTask, set its local iterator to the first subtask.
        if(isFirst)
        {
            iterator = _subTasks[subtaskId];
            iterator.Start();
            isFirst = false;
        }
        // Evaluate if the subtask is completed
        var val = iterator.IsCompleted();
        if (val.HasValue && val.Value)
        {
            // If it is complete, move to the next task
            iterator = iterator.NextTask();
            if (iterator == null)
            {
                Debug.Log("Subtask"+ subtaskId +" Completed");
                // If there are no more tasks, move to the next subtask if there is any left
                if (subtaskId < _subTasks.Count-1)
                {
                    subtaskId++;
                    iterator = _subTasks[subtaskId];
                    iterator.Start();
                }
                // No more subtasks, subtasktask completed
                else return true;
            }
        }
        // Subtask is not completed, return false.
        return false;
    }
}
[System.Serializable]
public class YesNoTask : Task
{
    //state = -1 -> no
    //state = 0 -> neutral
    //state = 1 -> true

    public int _state = 0;
    public override void Start()
    {
        base.Start();
    }

    public override bool? IsCompleted()
    {
        switch (_state)
        {
            case -1:
                return false;
            case 0:
                return null;
            case 1:
                return true;
        }
        return null;
    }

    public void SetState(int state)
    {
        _state = state;
    }

    public YesNoTask(string title, string description, GameObject animationObject, GameObject imageTargetObject, int id) : base(title, description, animationObject, imageTargetObject, id)
    {

    }
}
