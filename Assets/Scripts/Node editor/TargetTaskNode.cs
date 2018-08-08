using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Node_editor
{
    public class TargetTaskNode : Node
    {

        private NodeBasedEditor _nodeBasedEditor;

        Rect _targetObjectRect, _taskTitleRect, _taskDescriptionRect, _animationObjectRect;
        public TargetTaskData TargetTaskData;

        public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
        {
            TargetTaskData = null;
        
        }
        public TargetTaskNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, TargetTaskData t):base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint,OnClickRemoveNode)
        {

            TargetTaskData = t;
        }

        public void Initialize(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, TargetTaskData t)
        {
            base.Initialize(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle,
                OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
            TargetTaskData = t;
        }

        public override void Draw()
        {
            base.Draw();

            if (!_nodeBasedEditor)
            {
                _nodeBasedEditor = (NodeBasedEditor) Resources.FindObjectsOfTypeAll(typeof(NodeBasedEditor))[0];
            }
            _targetObjectRect = new Rect(rect.position.x + 11, rect.position.y + 10, 300, 20);
            _animationObjectRect = new Rect(_targetObjectRect.position.x, _targetObjectRect.position.y + _targetObjectRect.height, 300, 20);
            _taskTitleRect = new Rect(_animationObjectRect.position.x, _animationObjectRect.position.y + _animationObjectRect.height, 300, 20);     
            _taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);

            TargetTaskData._baseObject = EditorGUI.ObjectField(_targetObjectRect, "Target Object: ", TargetTaskData._baseObject , typeof(GameObject), true) as GameObject;
            TargetTaskData._animationObject  = EditorGUI.ObjectField(_animationObjectRect, "Animation Object: ", TargetTaskData._animationObject, typeof(GameObject), true) as GameObject;
            EditorGUI.BeginChangeCheck();
            TargetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",TargetTaskData._title);
            if (EditorGUI.EndChangeCheck())
            {
//                _nodeBasedEditor.OnBeforeSerialize();
//                TaskModel.Instance.OnBeforeSerialize();
            }
            TargetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",TargetTaskData._description);
        
        }
    }
}
