using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceOrientation : MonoBehaviour
{
	private bool first = true;
	
	
	void LateUpdate()
	{
		if(Screen.orientation != ScreenOrientation.Portrait)
			Screen.orientation = ScreenOrientation.Portrait;	
		
	}
}
