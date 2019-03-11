using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUp : MonoBehaviour
{
	public AnimationCurve animationCurve;
	public float animationSpeed;
	
	private Vector3 initialScale;
	private void OnEnable()
	{
		initialScale = transform.localScale;
		transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

		StartCoroutine(Move(transform.localScale, initialScale, animationCurve, animationSpeed));
	}

	
	IEnumerator Move(Vector3 scale1, Vector3 scale2, AnimationCurve ac, float time)
	{
		Application.targetFrameRate = 60;
		float timer = 0.0f;
		while (timer <= time)
		{
			transform.localScale = Vector3.Lerp(scale1, scale2, ac.Evaluate(timer / time));
			timer += Time.deltaTime;
			yield return null;
		}
		Application.targetFrameRate = 30;


	}
}
