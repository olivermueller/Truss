using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetElement : MonoBehaviour {

    public bool isActive = false;
    public List<CheckItem> checkItems;

    private bool prevActive = false;
   
    MeshRenderer meshRenderer;
    UIManager uiManager;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        uiManager = FindObjectOfType<Canvas>().gameObject.GetComponent<UIManager>();
    }

    
    private void Update()
    {
        prevActive = isActive;
        isActive = meshRenderer.enabled;
        if (isActive != prevActive && isActive)
        {
            TargetManager.Instance.currentlyActive = this.gameObject;
            TargetManager.Instance.canvas.enabled = true;
            foreach (CheckItem ci in checkItems)
            {
                uiManager.AddCheckItem(ci.name, ci.isCompleted, ci.animationObject, ci.requiredComponents);
            }

        }
        if(prevActive && !isActive)
        {
            TargetManager.Instance.currentlyActive = null;
            TargetManager.Instance.canvas.enabled = false;
            uiManager.RemoveAllCheckItems();
        }
        
       
         
    }
}
