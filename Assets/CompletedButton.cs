using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedButton : MonoBehaviour {
    private Button button;
    public int checkItemIndex;
    private Canvas canvas;

    void OnEnable()
    {
        Button btn = this.GetComponent<Button>();
        canvas = TargetManager.Instance.canvas;

        btn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        CheckItem ci = TargetManager.Instance.currentlyActive.GetComponent<TargetElement>().checkItems[checkItemIndex];
        ci.isCompleted = true;
        GameObject currentButton = canvas.gameObject.GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetChild(0).GetChild(checkItemIndex).gameObject;
        currentButton.transform.GetChild(2).GetComponent<Image>().sprite = canvas.GetComponent<UIManager>().correctTexture;

        foreach(GameObject go in currentButton.GetComponent<CheckItemLoader>().instantiatedComponents)
            Destroy(go);
        canvas.GetComponent<UIManager>().EnableScrollView();
        this.gameObject.SetActive(false);

        XAPIStatement statement = new XAPIStatement(TargetManager.Instance.username, "mailto:" + TargetManager.Instance.email, "passed", "http:∕∕adlnet.gov∕expapi∕verbs∕passed", "http:∕∕adlnet.gov∕expapi∕activities∕ARTruss", ci.name + " Test", "Completed " + ci.name);
        TargetManager.Instance.SEND(statement);
    }
    private void OnDisable()
    {
        
    }
}
