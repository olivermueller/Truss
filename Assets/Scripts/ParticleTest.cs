using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ParticleTest : MonoBehaviour
{

	public Transform startPoint, endPoint;
	public float particleSpeed = 10;
	public float rateOfChange;

	private void Start()
	{
		this.transform.position = startPoint.transform.position;
		this.transform.rotation = startPoint.transform.rotation;
		//rateOfChange = Vector3.Angle(startPoint.forward, (endPoint.position - startPoint.position).normalized)/1000.0f;
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

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "EndPoint")
		{
			gameObject.SetActive(false);
			transform.position = startPoint.transform.position;
			//rateOfChange = Vector3.Angle(startPoint.forward, (endPoint.position - startPoint.position).normalized)/1000.0f;
			gameObject.SetActive(true);
		}
	}
}
