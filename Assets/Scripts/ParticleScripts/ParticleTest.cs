using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ParticleTest : MonoBehaviour
{
	[HideInInspector]
	public Transform startPoint, endPoint;
	
	public float particleSpeed = 10;
	public float rateOfChange;
	public float angle;
	private void Start()
	{
		this.transform.position = startPoint.transform.position;
		this.transform.rotation = startPoint.transform.rotation;
		rateOfChange =Utilities.Remap(Vector3.Angle(startPoint.forward, (startPoint.position - endPoint.position).normalized),0.0f, 360.0f, 0.002f, 0.3f);
		angle = Vector3.Angle(startPoint.forward, (startPoint.position - endPoint.position).normalized);
		Destroy(gameObject, 5);
	}

	public float forwardIncrease = 1.0f, sideIncrease = 0.0f;
	// Use this for initialization
	private void Update()
	{
		Vector3 direction = startPoint.forward * forwardIncrease + (endPoint.position - transform.position).normalized *(1.0f-forwardIncrease);
		transform.forward = direction.normalized;
		transform.position += transform.forward * particleSpeed *Time.deltaTime;
		
		forwardIncrease -= rateOfChange;
		if (forwardIncrease < 0.0) forwardIncrease = 0.0f;
		
		if(Vector3.Distance(transform.position, endPoint.position)< 0.08f) Destroy(gameObject);
	}
}
