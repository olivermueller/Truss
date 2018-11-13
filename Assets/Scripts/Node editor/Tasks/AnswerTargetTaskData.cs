using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AnswerTargetTaskData : TaskData
{
    private bool targetActive;
	public GameObject yesPrefab, noPrefab;
    public GameObject instantiatedYesButton, instantiatedNoButton;
    public TaskData noTask;

    public void SetNoTask(TaskData no)
    {
        noTask = no;
    }

    public TaskData StartNoTask()
    {
        noTask.StartTask();
        return noTask;
    }
    
    private bool? _finished;
    
    private ParticleSpawner _particleSpawner;
    public override void StartTask()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.AddListener(IsTargetActive);
        //CreateButtons(true);
        if (goalPosition)
        {
            _particleSpawner = Camera.main.gameObject.AddComponent<ParticleSpawner>();
            _particleSpawner.endPoint = goalPosition;
            _particleSpawner.Begin();
        }
        
        _finished = null;
        base.StartTask();

    }

    public void CreateButtons(bool active)
    {
        if (active)
        {
            GameObject canvas = FindObjectOfType<Canvas>().gameObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
            instantiatedYesButton = Instantiate(yesPrefab);
            if(noTask != null)
            {
                instantiatedNoButton = Instantiate(noPrefab);
                instantiatedNoButton.transform.SetParent(canvas.transform);
                instantiatedNoButton.GetComponent<Button>().onClick.AddListener(NoButton);
            }
            else
            {
                instantiatedYesButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            }
            instantiatedYesButton.transform.SetParent(canvas.transform);

            instantiatedYesButton.GetComponent<Button>().onClick.AddListener(YesButton);
        }
        else
        {
            Destroy(instantiatedNoButton);
            Destroy(instantiatedYesButton);
        }
        
    }

    public void YesButton()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.RemoveListener(IsTargetActive);
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = true;
    }
    
    public void NoButton()
    {
        _baseObject.GetComponent<MissionTrackableEventHandler>().OnTrackableStateChange.RemoveListener(IsTargetActive);
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = false;
    }
    public override bool? IsCompleted()
    {
        
        if(_finished != null) Destroy(_particleSpawner);
        return _finished;
    }
    public void IsTargetActive(bool active)
    {
        targetActive = active;
        Debug.LogWarning("Callback made on " + _title + "Value: " + active);
        CreateButtons(active);
    }
}
