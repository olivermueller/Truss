using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
	public float animationTime;
	public AnimationCurve animationCurve;
	private RectTransform rectTransform;

	private bool isAnimating;
	private Vector3 startPosition, endPosition;
	
	private bool animatingUp;

	private void Start()
	{
		startPosition = transform.position;
		endPosition = startPosition + new Vector3(0, Screen.height / 2.0f - Screen.height / 4.0f, 0);
	}

	public void MainPanelAnimationUp()
	{
		StopCoroutine(nameof(Move));
		StartCoroutine(Move(transform.position, endPosition, animationCurve, animationTime));
	}
	public void MainPanelAnimationDown()
	{
		StopCoroutine(nameof(Move));
		StartCoroutine(Move(transform.position, startPosition, animationCurve, animationTime));
	}

	IEnumerator Move(Vector3 pos1, Vector3 pos2, AnimationCurve ac, float time)
	{
		isAnimating = true;
		Application.targetFrameRate = 60;
		float timer = 0.0f;
		while (timer <= time)
		{
			transform.position = Vector3.Lerp(pos1, pos2, ac.Evaluate(timer / time));
			timer += Time.deltaTime;
			yield return null;
		}
		Application.targetFrameRate = 30;
		isAnimating = false;
	}
}
