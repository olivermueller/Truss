using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
	private Renderer _renderer;
	// Use this for initialization
	void Start ()
	{
		_renderer = GetComponent<Renderer>();
	}

	private void OnTriggerEnter(Collider other)
	{
		_renderer.enabled = false;
	}

	private void OnTriggerExit(Collider other)
	{
		_renderer.enabled = true;
	}
}
