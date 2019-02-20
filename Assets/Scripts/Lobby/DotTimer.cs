using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotTimer : MonoBehaviour
{
	private Text textElement;

	private void OnEnable()
	{
		textElement = GetComponentInChildren<Text>();
		StartCoroutine(AddDot());
	}

	IEnumerator AddDot()
	{
		int count = 0;
		do
		{
			count++;
			textElement.text += ".";
			yield return new WaitForSeconds(1);
		} while (count<5);
		
	}

	private void OnDisable()
	{
		textElement.text = "CONNECTING";
		StopCoroutine(AddDot());
	}
}
