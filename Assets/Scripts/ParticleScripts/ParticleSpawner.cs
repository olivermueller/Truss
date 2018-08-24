using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParticleSpawner : MonoBehaviour
{
	public float spawnTime = 1;
	public GameObject particleSystemPrefab, endpoint;
	void Start () 
	{
		InvokeRepeating("SpawnParticles", 0, spawnTime);
	}

	void SpawnParticles()
	{
		ParticleTest particleScript = Instantiate(particleSystemPrefab).GetComponent<ParticleTest>();
		particleScript.startPoint = transform;
		particleScript.endPoint = endpoint.transform;
	}
}
