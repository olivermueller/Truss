using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class StartTaskNode : Node {

    private StartTaskData _targetTaskData;
    
    Rect _taskTitleRect;

    private void OnEnable()
    {
        _targetTaskData = TaskData as StartTaskData;
        
    }

    public override void Draw()
    {
        base.Draw();
        if (_targetTaskData==null)
        {
            _targetTaskData = TaskData as StartTaskData;
        }

        inPoint.enabled = false;
        
        _taskTitleRect = new Rect(rect.position.x + 50, rect.position.y + 50, 300, 20); 
    
    }
}
#endif