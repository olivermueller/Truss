using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class NestedTaskNode : Node {
    public ConnectionPoint listPoint;

    private NestedTaskData _targetTaskData;
    
    Rect _taskTitleRect, _taskDescriptionRect;

    private void OnEnable()
    {
        _targetTaskData = TaskData as NestedTaskData;
        
        
    }

    private void OnClickListPoint(ConnectionPoint obj)
    {
        Debug.Log("Clicked Nested node");
        TaskModel.Instance.selectedOutPoint = obj;
    }

    public override void Draw()
    {
        base.Draw();
        
        if (_targetTaskData==null)
        {
            _targetTaskData = TaskData as NestedTaskData;
        }

        if (listPoint == null)
        {
            listPoint = gameObject.AddComponent<ConnectionPoint>();
            listPoint.Initialize(this, ConnectionPointType.Nested, inPoint.style, OnClickListPoint);
            listPoint.rect.width = 20;
            listPoint.rect.height = 10;
        }

        if (listPoint.OnClickConnectionPoint == null)
            listPoint.OnClickConnectionPoint = OnClickListPoint;
        
        
        listPoint.Draw();
        
        _taskTitleRect = new Rect(rect.position.x + 50, rect.position.y + 50, 300, 20);     
        _taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);

        _targetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTaskData._title);
        _targetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTaskData._description);
    
    }
    
}
#endif