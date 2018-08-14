using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AnswerTaskData : TaskData
{
    public GameObject yesPrefab, noPrefab;
    public GameObject instantiatedYesButton, instantiatedNoButton;

    private bool? _finished;
        public override void StartTask()
        {
            base.StartTask();
            GameObject canvas = FindObjectOfType<Canvas>().gameObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
            instantiatedYesButton = Instantiate(yesPrefab);
            instantiatedNoButton = Instantiate(noPrefab);
            instantiatedYesButton.transform.SetParent(canvas.transform);
            instantiatedNoButton.transform.SetParent(canvas.transform);
            instantiatedNoButton.GetComponent<Button>().onClick.AddListener(NoButton);
            instantiatedYesButton.GetComponent<Button>().onClick.AddListener(YesButton);


        }

    public void YesButton()
    {
        Destroy(instantiatedNoButton);
        Destroy(instantiatedYesButton);
        _finished = true;
    }
    
    public void NoButton()
    {
        _finished = false;
    }
        public override bool? IsCompleted()
        {
            return _finished;
        }

}
