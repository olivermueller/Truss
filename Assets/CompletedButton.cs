using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedButton : MonoBehaviour {
    private Button button;
    public int checkItemIndex;
    public bool isYes;
    private Canvas canvas;

    void OnEnable()
    {
        canvas = TargetManager.Instance.canvas;
    }

    public void TaskOnClick()
    {
        Debug.Log("Called From: " + this.gameObject.name );
        CheckItem ci = TargetManager.Instance.currentlyActive.GetComponent<TargetElement>().checkItems[checkItemIndex];
        
        GameObject currentButton = canvas.gameObject.GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetChild(0).GetChild(checkItemIndex).gameObject;
        
        foreach (GameObject go in currentButton.GetComponent<CheckItemLoader>().instantiatedComponents)
            Destroy(go);
        UIManager um = canvas.GetComponent<UIManager>();
        if (ci.subTask.Count == 0 && isYes)
        {
            CompletedTask(currentButton, um, ci);
        }
        else if (ci.subTask.Count > 0 && isYes)
        {
           
            if (ci.subTaskIndex < ci.subTask.Count)
            {
                Debug.Log("yas" + ci.subTaskIndex);
                um.question.text = ci.subTask[ci.subTaskIndex].question;
                ci.subTaskIndex++;
            }
            else
            {
                CompletedTask(currentButton, um, ci);
            }
        }
        else
        {
            CompletedTask(currentButton, um, ci);
        }
        

      

        XAPIStatement statement = new XAPIStatement(TargetManager.Instance.username, "mailto:" + TargetManager.Instance.email, "passed", "http:∕∕adlnet.gov∕expapi∕verbs∕passed", "http:∕∕adlnet.gov∕expapi∕activities∕ARTruss", ci.name + " Test", "Completed " + ci.name);
        TargetManager.Instance.SEND(statement);
    }

    void CompletedTask(GameObject currentButton, UIManager um, CheckItem ci)
    {
        if (isYes)
        {
            ci.isCompleted = true;
            currentButton.transform.GetChild(2).GetComponent<Image>().sprite = canvas.GetComponent<UIManager>().correctTexture;
        }

        um.EnableScrollView();
        um.yesNoMenu.SetActive(false);
        ci.subTaskIndex = 0;
    }
}
