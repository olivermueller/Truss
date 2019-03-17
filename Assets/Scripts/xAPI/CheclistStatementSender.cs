using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheclistStatementSender : MonoBehaviour
{
	private Toggle toggle;
	public int ID;
 	void Start ()
	 {
		 toggle = GetComponent<Toggle>();
		 
		 toggle.onValueChanged.AddListener(delegate {
			 ToggleValueChanged(toggle);
		 });
			
		 ID = transform.GetSiblingIndex();
		 
		 //XAPIManager.instance.AddToQueue(ID,"http://id.tincanapi.com/activitytype/checklist-item", "not completed", "http://activitystrea.ms/schema/1.0/application"+GetComponentInChildren<TextMeshProUGUI>().text);

	 }

	void ToggleValueChanged(Toggle change)
	{

		var iterator = FindObjectOfType<TestingScript>().iterator;
		if (change.isOn)
		{
			XAPIManager.instance.Send("http://activitystrea.ms/schema/1.0/accept", "accepted", "Trainer", "http://example.com/node/" + iterator.XapiID + "/" + "checklistitem/" + transform.GetSiblingIndex()+1);
		}
		else
		{
			XAPIManager.instance.Send("http://activitystrea.ms/schema/1.0/reject", "rejected", "Trainer", "http://example.com/node/" + iterator.XapiID + "/" + "checklistitem/" + transform.GetSiblingIndex()+1);
		}
	}
}
