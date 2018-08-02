using System;
using UnityEditor;
using UnityEngine;
[System.Serializable]
public class TargetTaskNode : Node {



    Rect targetObjectRect, taskTitleRect, taskDescriptionRect, animationObjectRect;
    public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
    {
        targetTask = null;
        
    }
    public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, TargetTask t):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
    {

        targetTask = t;
    }
    
    TargetTask targetTask;
    
    public override void Draw()
    {
        base.Draw();

        
        targetObjectRect = new Rect(rect.position.x + 11, rect.position.y + 10, 300, 20);
        animationObjectRect = new Rect(targetObjectRect.position.x, targetObjectRect.position.y + targetObjectRect.height, 300, 20);
        taskTitleRect = new Rect(animationObjectRect.position.x, animationObjectRect.position.y + animationObjectRect.height, 300, 20);     
        taskDescriptionRect = new Rect(taskTitleRect.position.x, taskTitleRect.position.y + taskTitleRect.height, 300, 20);

        targetTask._baseObject = EditorGUI.ObjectField(targetObjectRect, "Target Object: ", targetTask._baseObject , typeof(GameObject), true) as GameObject;
        targetTask._animationObject  = EditorGUI.ObjectField(animationObjectRect, "Animation Object: ", targetTask._animationObject, typeof(GameObject), true) as GameObject;

        targetTask._title = EditorGUI.TextField(taskTitleRect, "Title",targetTask._title);
        targetTask._description = EditorGUI.TextField(taskDescriptionRect, "Description",targetTask._description);
        
    }
}
