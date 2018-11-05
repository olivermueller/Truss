using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite correctTexture, wrongTexture;
    public GameObject checkItemPrefab;
    public GameObject mainPanelObj;
    public GameObject yesButton, noButton, yesNoMenu;
    public Text question;
    private GameObject contentGameObj;

    
    private void Start()
    {
        contentGameObj = GameObject.FindObjectOfType<VerticalLayoutGroup>().gameObject;
    }

    public void AddCheckItem(string taskName, bool isCompleted, GameObject animationObject, List<GameObject> requiredComponents)
    {
        GameObject tempCheckItem = Instantiate(checkItemPrefab);
        tempCheckItem.transform.SetParent(contentGameObj.transform);
        tempCheckItem.transform.localScale = new Vector3(1, 1, 1);
        tempCheckItem.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = taskName;
        Image completedImage = tempCheckItem.transform.GetChild(2).GetComponent<Image>();
        if (isCompleted) completedImage.sprite = correctTexture;
        else completedImage.sprite = wrongTexture;
        tempCheckItem.GetComponent<CheckItemLoader>().animationObject = animationObject;
        tempCheckItem.GetComponent<CheckItemLoader>().requiredComponents = requiredComponents;
    }

    public void RemoveAllCheckItems()
    {
        for(int i=0; i<contentGameObj.transform.childCount; i++)
        {
            Destroy(contentGameObj.transform.GetChild(i).gameObject);
        }
    }

    public void DisableScrollView()
    {
        mainPanelObj.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }
    public void EnableScrollView()
    {
        mainPanelObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

}
