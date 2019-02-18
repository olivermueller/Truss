using UnityEngine;
//using UnityEngine.XR.iOS;

public class ObjectPlacement : MonoBehaviour
{
//	public GameObject ObjectPrefab;
//	
//	private void CreateObject(Vector3 position)
//	{
//		var cam = Camera.main.gameObject.transform.rotation.eulerAngles;
//		var rotation = Quaternion.Euler(0, cam.y, 0);
//		Instantiate (ObjectPrefab, position, rotation);
//	}
//
//	// Update is called once per frame
//	private void Update () {
//		if (Input.touchCount <= 0) return;
//		var touch = Input.GetTouch(0);
//		if (touch.phase != TouchPhase.Began) return;
//		var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
//		var point = new ARPoint {
//			x = screenPosition.x,
//			y = screenPosition.y
//		};
//						
//		var hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
//			ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
//		if (hitResults.Count <= 0) return;
//		var position = UnityARMatrixOps.GetPosition (hitResults[0].worldTransform);
//		CreateObject (new Vector3 (position.x, position.y, position.z));
//		Destroy(gameObject);
//	}
}
