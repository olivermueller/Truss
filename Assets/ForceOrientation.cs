using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceOrientation : MonoBehaviour
{
	private bool first = true;

	void Update()
	{
		if (first)
		{
			first = false;
			Screen.orientation = ScreenOrientation.Portrait;	
		}
		
	}
}
