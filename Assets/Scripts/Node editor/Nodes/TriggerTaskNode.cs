using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class TriggerTaskNode : Node {



//    private NodeBasedEditor _nodeBasedEditor;

    private TriggerTaskData _targetTaskData;
    
    Rect _targetObjectRect, _taskTitleRect, _taskDescriptionRect, _animationObjectRect;

    private void OnEnable()
    {
        _targetTaskData = TaskData as TriggerTaskData;
    }

    public override void Draw()
    {
        base.Draw();
        if (_targetTaskData==null)
        {
            _targetTaskData = TaskData as TriggerTaskData;
        }
//        if (!_nodeBasedEditor)
//        {
//            _nodeBasedEditor = (NodeBasedEditor) Resources.FindObjectsOfTypeAll(typeof(NodeBasedEditor))[0];
//        }
        _targetObjectRect = new Rect(rect.position.x + 50, rect.position.y + 50, 300, 20);
        _animationObjectRect = new Rect(_targetObjectRect.position.x, _targetObjectRect.position.y + _targetObjectRect.height, 300, 20);
        _taskTitleRect = new Rect(_animationObjectRect.position.x, _animationObjectRect.position.y + _animationObjectRect.height, 300, 20);     
        _taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);

        _targetTaskData._baseObject = EditorGUI.ObjectField(_targetObjectRect, "Target Object: ", _targetTaskData._baseObject , typeof(GameObject), true) as GameObject;
        _targetTaskData._animationObject  = EditorGUI.ObjectField(_animationObjectRect, "Animation Object: ", _targetTaskData._animationObject, typeof(GameObject), true) as GameObject;
        EditorGUI.BeginChangeCheck();
        _targetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTaskData._title);
        if (EditorGUI.EndChangeCheck())
        {
//                _nodeBasedEditor.OnBeforeSerialize();
//                TaskModel.Instance.OnBeforeSerialize();
        }
        _targetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTaskData._description);
    
    }
}
#endif