using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParticleSpawner : MonoBehaviour
{
	public float spawnTime = 3.0f, distanceFromSource = 0.2f, percentPerSecond = 0.65f;
	public GameObject particleSystemPrefab;
	public Transform endPoint;
//	private void Start()
//	{
//		Begin();
//	}

	public void Begin() 
	{
		particleSystemPrefab = Resources.Load("ParticleSystemPrefab") as GameObject;
		InvokeRepeating("SpawnParticles", 0, spawnTime);
	}
	
	void SpawnParticles()
	{
		GameObject go = Instantiate(particleSystemPrefab);
		FollowPoints followScript =  go.GetComponent<FollowPoints>();
		followScript.waypointArray = new Transform[3];
		followScript.waypointArray[0] = transform;
		
		GameObject middlePoint = new GameObject();
		middlePoint.transform.position = transform.position + transform.forward * distanceFromSource;
		followScript.waypointArray[1] = middlePoint.transform;
		followScript.waypointArray[2] = endPoint;
		followScript.percentsPerSecond = percentPerSecond;
		Destroy(middlePoint, 10);
	}
}
