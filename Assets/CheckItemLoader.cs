using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckItemLoader : MonoBehaviour {

    public GameObject animationObject;
    public Button button;

    public GameObject instantiatedAnimObject;
    private Canvas canvas;
    void OnEnable()
    {
        Button btn = button.GetComponentInChildren<Button>();
        canvas = GameObject.FindObjectOfType<Canvas>();

        btn.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        instantiatedAnimObject = GameObject.Instantiate(animationObject, TargetManager.Instance.currentlyActive.transform);
        canvas.GetComponent<UIManager>().DisableScrollView();
        canvas.GetComponent<UIManager>().completedButton.SetActive(true);
        canvas.GetComponent<UIManager>().completedButton.GetComponent<CompletedButton>().checkItemIndex = transform.GetSiblingIndex();
    }
    private void OnDisable()
    {
        canvas.GetComponent<UIManager>().EnableScrollView();
        if (instantiatedAnimObject) Destroy(instantiatedAnimObject);
    }
}
