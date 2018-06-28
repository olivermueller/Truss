using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckItemLoader : MonoBehaviour {

    public GameObject animationObject;
    public List<GameObject> requiredComponents;
    public Button button;

    public List<GameObject> instantiatedComponents;
    private Canvas canvas;
    void OnEnable()
    {
        Button btn = button.GetComponentInChildren<Button>();
        canvas = TargetManager.Instance.canvas;

        btn.onClick.AddListener(TaskOnClick);
        instantiatedComponents = new List<GameObject>();
    }

    void TaskOnClick()
    {
        //instantiate animated object
        instantiatedComponents.Add(GameObject.Instantiate(animationObject, TargetManager.Instance.currentlyActive.transform));

        //instantiate other objects
        foreach(GameObject go in requiredComponents)
        {
            GameObject tempObj = GameObject.Instantiate(go, TargetManager.Instance.currentlyActive.transform);
            tempObj.GetComponent<Animation>().enabled = false;
            instantiatedComponents.Add(tempObj);
        }

        canvas.GetComponent<UIManager>().DisableScrollView();
        canvas.GetComponent<UIManager>().completedButton.SetActive(true);
        canvas.GetComponent<UIManager>().completedButton.GetComponent<CompletedButton>().checkItemIndex = transform.GetSiblingIndex();
    }
    private void OnDisable()
    {
        canvas.GetComponent<UIManager>().EnableScrollView();
        canvas.GetComponent<UIManager>().completedButton.SetActive(false);
        foreach(GameObject go in instantiatedComponents)
            Destroy(go);
    }
}
