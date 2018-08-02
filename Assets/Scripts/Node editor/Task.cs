using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    protected string _title, _description;
    protected Task _next;
    public GameObject _animationObject, _baseObject, _instantiatedAnimationObject;

    public void SetNextTask(Task next)
    {
        _next = next;   
    }
    public virtual void Start()
    {
        Debug.Log("Starte" + _title);
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
        return _next;
    }

    public Task(string title, string description, GameObject animationObject, GameObject imageTargetObject)
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

public class TargetTask:Task
{
    
    Renderer _renderer;
    public override void Start()
    {
        base.Start();
        _renderer = _baseObject.GetComponentInChildren(typeof(Renderer), true) as Renderer;
    }

    public override bool? IsCompleted()
    {
        return IsImageTargetActive();
    }

    public TargetTask(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
    {
        
    }
    bool IsImageTargetActive()
    {
        return _renderer.enabled;
    }

}

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

    public YesNoTask(string title, string description, GameObject animationObject, GameObject imageTargetObject) : base(title, description, animationObject, imageTargetObject)
    {

    }
}
