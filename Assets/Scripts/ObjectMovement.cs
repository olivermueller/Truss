using UnityEngine;
using UnityEngine.XR.iOS;

public class ObjectMovement : MonoBehaviour
{
	public Canvas canvas;
	private void Update () {
//		if (canvas.enabled) return;
		if (Input.touchCount <= 0) return;
		var touch = Input.GetTouch(0);
		if (touch.phase != TouchPhase.Moved) return;
		var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
		var point = new ARPoint {
			x = screenPosition.x,
			y = screenPosition.y
		};
						
		var hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
			ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
		if (hitResults.Count <= 0) return;
		var position = UnityARMatrixOps.GetPosition (hitResults[0].worldTransform);
		transform.position = position;
	}
}
