using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class MissionTrackableEventHandler : DefaultTrackableEventHandler
{
	
	public OnTrackableStateChanged OnTrackableStateChange;
	/// <summary>
	///     Implementation of the ITrackableEventHandler function called when the
	///     tracking state changes.
	/// </summary>
	public override void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			OnTrackableStateChange.Invoke(true);
			OnTrackingFound();
		}
		else if ((previousStatus == TrackableBehaviour.Status.TRACKED &&
		         newStatus == TrackableBehaviour.Status.NO_POSE)||newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			OnTrackableStateChange.Invoke(false);
			OnTrackingLost();
		}
		else
		{
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			OnTrackableStateChange.Invoke(false);
			OnTrackingLost();
		}
	}
}
[System.Serializable]
public class OnTrackableStateChanged : UnityEvent<bool>
{
}