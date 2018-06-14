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
        canvas = GameObject.FindObjectOfType<Canvas>();

        btn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        CheckItem ci = TargetManager.Instance.currentlyActive.GetComponent<TargetElement>().checkItems[checkItemIndex];
        ci.isCompleted = true;
        GameObject currentButton = canvas.gameObject.GetComponentInChildren<ScrollRect>().transform.GetChild(0).GetChild(0).GetChild(checkItemIndex).gameObject;
        currentButton.transform.GetChild(1).GetComponent<Image>().sprite = canvas.GetComponent<UIManager>().correctTexture;
        Destroy(currentButton.GetComponent<CheckItemLoader>().instantiatedAnimObject);
        canvas.GetComponent<UIManager>().EnableScrollView();
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        
    }
}
