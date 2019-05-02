using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OffscreenArrow : MonoBehaviour
{
	/*
	 * Script for offscreen arrow following an object position.
	 * 
	 */
	
	public List<GameObject> Targets;	//objects to be followed by arrows
	public GameObject ArrowUIPrefab;	//UI element used for displaying arrow; assumes the arrow prefab is pointing to the left
	public GameObject SelectionUIPrefab;	//UI element used for displaying selection when target is in camera view; 
	public float PaddingX, PaddingY;

	private float _viewportPaddingX, _viewportPaddingY;
	private GameObject[] _instantiatedArrows;
	private GameObject[] _instantiatedSelections;
	private Canvas _canvas;
	void Start ()
	{
		_viewportPaddingX = 1.0f - (Screen.width - PaddingX)/Screen.width;
		_viewportPaddingY = 1.0f - (Screen.height - PaddingY)/Screen.height;
		
		_canvas = FindObjectOfType<Canvas>();
		if (_canvas == null)
		{
			_canvas = (new GameObject()).AddComponent<Canvas>();

		}
		
		_instantiatedArrows = new GameObject[Targets.Count];
		_instantiatedSelections = new GameObject[Targets.Count];
		for (int i=0; i<Targets.Count; i++)
		{
			var instantiatedArrow = Instantiate(ArrowUIPrefab);
			instantiatedArrow.transform.parent = _canvas.transform;
			_instantiatedArrows[i] = instantiatedArrow;
			
			var instantiatedSelection = Instantiate(SelectionUIPrefab);
			instantiatedSelection.transform.parent = _canvas.transform;
			_instantiatedSelections[i] = instantiatedSelection;
			_instantiatedSelections[i].SetActive(false);
			
		}
	}

	private float _arrowRotation;
	void Update () 
	{
		for (int i=0; i<Targets.Count; i++)
		{
			Vector3 targetViewportPosition = Camera.main.WorldToViewportPoint(Targets[i].transform.position);

			//take sign on z axis to know whether the camera is pointing away from the target
			var sign = Math.Sign(targetViewportPosition.z);
			targetViewportPosition.x *= sign;
			targetViewportPosition.y *= sign;
			
			if(TestRange(0, 1, targetViewportPosition.x) && TestRange(0, 1, targetViewportPosition.y))
			{
				//print("Is In!!");
				_instantiatedSelections[i].SetActive(true);
				_instantiatedArrows[i].SetActive(false);
			}
			else
			{
				_instantiatedSelections[i].SetActive(false);
				_instantiatedArrows[i].SetActive(true);
			}
			
			Vector2 cameraTargetVec = (targetViewportPosition - Camera.main.WorldToViewportPoint(transform.position)).normalized;
			
			_arrowRotation = 0;
			if (targetViewportPosition.x < _viewportPaddingX)
			{
				_arrowRotation = 0;
				targetViewportPosition.x = _viewportPaddingX;	
			}
			
			if (targetViewportPosition.x > 1 - _viewportPaddingX)
			{
				_arrowRotation = 180;
				targetViewportPosition.x = 1 - _viewportPaddingX;
			}
			
			if (targetViewportPosition.y < _viewportPaddingY)
			{
				_arrowRotation = 90;
				targetViewportPosition.y = _viewportPaddingY;
			}
			
			if (targetViewportPosition.y > 1 - _viewportPaddingY)
			{
				_arrowRotation = -90;
				targetViewportPosition.y = 1 - _viewportPaddingY;
			}

			

			_instantiatedArrows[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, _arrowRotation);
			_instantiatedArrows[i].GetComponent<RectTransform>().position = Camera.main.ViewportToScreenPoint(targetViewportPosition);			
			_instantiatedSelections[i].GetComponent<RectTransform>().position = Camera.main.ViewportToScreenPoint(targetViewportPosition);

			
		}
	}
	
	
	
	bool TestRange(float min, float max, float val)
	{
		return val > min && val < max;
	}
}
