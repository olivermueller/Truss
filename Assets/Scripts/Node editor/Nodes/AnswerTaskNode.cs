using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class AnswerTaskNode : Node {

	private AnswerTaskData _targetTaskData;
	public ConnectionPoint _noPoint;
    private Rect _taskTitleRect, _taskDescriptionRect, _noButtonRect, _yesButtonRect, _goalPositionRect,_animationObjectRect,_targetObjectRect;

	private void OnEnable()
	{
		_targetTaskData = TaskData as AnswerTaskData;
	}

	public override void Draw()
	{
		base.Draw();
		if (_targetTaskData==null)
		{
			_targetTaskData = TaskData as AnswerTaskData;
		}
		if (_noPoint == null)
		{
			_noPoint = gameObject.AddComponent<ConnectionPoint>();
			_noPoint.Initialize(this, ConnectionPointType.Answer, inPoint.style, OnClickListPoint);
			_noPoint.rect.width = 60;
			_noPoint.rect.height = 50;
		}
		
		if (_noPoint.OnClickConnectionPoint == null)
			_noPoint.OnClickConnectionPoint = OnClickListPoint;
		
		_noPoint.Draw();
		
		_taskTitleRect = new Rect(rect.position.x + 50, rect.position.y + 50, 300, 20);     
		_taskDescriptionRect = new Rect(_taskTitleRect.position.x, _taskTitleRect.position.y + _taskTitleRect.height, 300, 20);
		_noButtonRect =  new Rect(_taskDescriptionRect.position.x, _taskDescriptionRect.position.y + _taskDescriptionRect.height, 300, 20);
		_yesButtonRect =  new Rect(_noButtonRect.position.x, _noButtonRect.position.y + _noButtonRect.height, 300, 20);
        _goalPositionRect = new Rect(_yesButtonRect.position.x, _yesButtonRect.position.y + _yesButtonRect.height, 300, 20);
		_animationObjectRect = new Rect(_goalPositionRect.position.x,_goalPositionRect.position.y + _goalPositionRect.height, 300, 20);
		_targetObjectRect = new Rect(_animationObjectRect.position.x,_animationObjectRect.position.y + _animationObjectRect.height, 300, 20);
		
		_targetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTaskData._title);
		_targetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTaskData._description);
		_targetTaskData.yesPrefab  = EditorGUI.ObjectField(_yesButtonRect, "Yes button Prefab: ", _targetTaskData.yesPrefab, typeof(GameObject), true) as GameObject;
		_targetTaskData.noPrefab  = EditorGUI.ObjectField(_noButtonRect, "No button Prefab: ", _targetTaskData.noPrefab, typeof(GameObject), true) as GameObject;
        _targetTaskData.goalPosition = EditorGUI.ObjectField(_goalPositionRect, "Goal Transform: ", _targetTaskData.goalPosition, typeof(Transform), true) as Transform;
		_targetTaskData._animationObject  = EditorGUI.ObjectField(_animationObjectRect, "Animation Object: ", _targetTaskData._animationObject, typeof(GameObject), true) as GameObject;
		_targetTaskData._baseObject = EditorGUI.ObjectField(_targetObjectRect, "Target Object: ", _targetTaskData._baseObject , typeof(GameObject), true) as GameObject;
	}
	
	
	private void OnClickListPoint(ConnectionPoint obj)
	{
		Debug.Log("Clicked Nested node");
		TaskModel.Instance.selectedOutPoint = obj;
	}
}
#endif
