using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class AnswerTaskNode : Node {

	private AnswerTaskData _targetTaskData;
	public ConnectionPoint _noPoint;
	private Rect _taskTitleRect, _taskDescriptionRect, _noButtonRect, _yesButtonRect;

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
		
		
		_targetTaskData._title = EditorGUI.TextField(_taskTitleRect, "Title",_targetTaskData._title);
		_targetTaskData._description = EditorGUI.TextField(_taskDescriptionRect, "Description",_targetTaskData._description);
		_targetTaskData.yesPrefab  = EditorGUI.ObjectField(_yesButtonRect, "Yes button Prefab: ", _targetTaskData.yesPrefab, typeof(GameObject), true) as GameObject;
		_targetTaskData.noPrefab  = EditorGUI.ObjectField(_noButtonRect, "No button Prefab: ", _targetTaskData.noPrefab, typeof(GameObject), true) as GameObject;
    
	}
	
	
	private void OnClickListPoint(ConnectionPoint obj)
	{
		Debug.Log("Clicked Nested node");
		TaskModel.Instance.selectedOutPoint = obj;
	}
}
#endif
