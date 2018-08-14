using System;
using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class TargetTaskNode : Node
{



    private TargetTaskData _targetTaskData;
    
    Rect _targetObjectRect, _taskTitleRect, _taskDescriptionRect, _animationObjectRect;

    private void OnEnable()
    {
        _targetTaskData = TaskData as TargetTaskData;
        
    }

    public override void Draw()
    {
        base.Draw();
        if (_targetTaskData==null)
        {
            _targetTaskData = TaskData as TargetTaskData;
        }

        _targetObjectRect = new Rect(rect.position.x + 50, rect.position.y + 50, 300, 20);
        _animationObjectRect = new Rect(_targetObjectRect.position.x, _targetObjectRect.position.y + _targetObjectRect.height, 300, 20);
        _taskTitleRect = new Rect(_animationObjectRect.position.x, _animationObjectRect.position.y + _animationObjectRect.height, 300, 20);     
        _taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);

        _targetTaskData._baseObject = EditorGUI.ObjectField(_targetObjectRect, "Target Object: ", _targetTaskData._baseObject , typeof(GameObject), true) as GameObject;
        _targetTaskData._animationObject  = EditorGUI.ObjectField(_animationObjectRect, "Animation Object: ", _targetTaskData._animationObject, typeof(GameObject), true) as GameObject;
        _targetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTaskData._title);
        _targetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTaskData._description);
    
    }
}
#endif