using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoints : MonoBehaviour {

	public Transform[] waypointArray;
	public float percentsPerSecond = 0.02f; // %2 of the path moved per second
	public float currentPathPercent = 0.0f; //min 0, max 1
         
	void Update () 
	{
		if (waypointArray != null)
		{
			currentPathPercent += percentsPerSecond * Time.deltaTime;
			iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
			if (currentPathPercent >= 1) Destroy(gameObject);
		}
	}
     
	void OnDrawGizmos()
	{
		//Visual. Not used in movement
		iTween.DrawPath(waypointArray);
	}
	
}
