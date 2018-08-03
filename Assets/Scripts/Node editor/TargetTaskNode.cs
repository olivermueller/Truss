using System;
using UnityEditor;
using UnityEngine;

namespace Node_editor
{
    [System.Serializable]
    public class TargetTaskNode : Node {



        Rect _targetObjectRect, _taskTitleRect, _taskDescriptionRect, _animationObjectRect;
        public TargetTask _targetTask;

        public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
        {
            _targetTask = null;
        
        }
        public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, TargetTask t):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
        {

            _targetTask = t;
        }
    
    
        public override void Draw()
        {
            base.Draw();

        
            _targetObjectRect = new Rect(rect.position.x + 11, rect.position.y + 10, 300, 20);
            _animationObjectRect = new Rect(_targetObjectRect.position.x, _targetObjectRect.position.y + _targetObjectRect.height, 300, 20);
            _taskTitleRect = new Rect(_animationObjectRect.position.x, _animationObjectRect.position.y + _animationObjectRect.height, 300, 20);     
            _taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);

            _targetTask._baseObject = EditorGUI.ObjectField(_targetObjectRect, "Target Object: ", _targetTask._baseObject , typeof(GameObject), true) as GameObject;
            _targetTask._animationObject  = EditorGUI.ObjectField(_animationObjectRect, "Animation Object: ", _targetTask._animationObject, typeof(GameObject), true) as GameObject;

            _targetTask._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTask._title);
            _targetTask._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTask._description);
        
        }
    }
}
