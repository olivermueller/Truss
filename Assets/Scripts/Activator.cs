using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
	public GameObject Arrow;

	public Animator Animator;

	public TargetElement element;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("MainCamera")) return;
		Arrow.SetActive(false);
//		Animator.gameObject.SetActive(true);
//		Animator.SetBool("animate", true);
		element.isActive = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("MainCamera")) return;
		Arrow.SetActive(true);
//		Animator.SetBool("animate", false);
//		Animator.gameObject.SetActive(false);
		element.isActive = false;
	}
}
