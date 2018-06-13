using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite correctTexture, wrongTexture;
    public GameObject checkItemPrefab;
    private GameObject contentGameObj;

    private void Start()
    {
        contentGameObj = GameObject.FindObjectOfType<VerticalLayoutGroup>().gameObject;
    }

    public void AddCheckItem(string taskName, bool isCompleted)
    {
        GameObject tempCheckItem = Instantiate(checkItemPrefab);
        tempCheckItem.transform.parent = contentGameObj.transform;
        tempCheckItem.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = taskName;
        Image completedImage = tempCheckItem.transform.GetChild(1).GetComponent<Image>();
        if (isCompleted) completedImage.sprite = correctTexture;
        else completedImage.sprite = wrongTexture;
    }

    public void RemoveAllCheckItems()
    {
        for(int i=0; i<contentGameObj.transform.childCount; i++)
        {
            Destroy(contentGameObj.transform.GetChild(i).gameObject);
        }
    }


}
