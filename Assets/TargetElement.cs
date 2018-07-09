﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetElement : MonoBehaviour {

    public bool isActive = false;
    public List<CheckItem> checkItems;

    public bool prevActive = false;
    public bool isFullyVirtual;
   
    MeshRenderer meshRenderer;
    UIManager uiManager;
    private void Start()
    {
        if(gameObject.tag == "ImageTarget")meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        else if(gameObject.tag == "CylinderTarget") meshRenderer = GetComponent<MeshRenderer>();
        uiManager = TargetManager.Instance.canvas.gameObject.GetComponent<UIManager>();
    }

    public bool status;
    private void Update()
    {
        status = meshRenderer.enabled;
        if (!isFullyVirtual)
        {
            prevActive = isActive;
            if (meshRenderer)
            {
                isActive = meshRenderer.enabled;
            }
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
        else
        {
            if (isActive != prevActive && isActive)
            {
                TargetManager.Instance.currentlyActive = this.gameObject;
                TargetManager.Instance.canvas.enabled = true;
                foreach (CheckItem ci in checkItems)
                {
                    uiManager.AddCheckItem(ci.name, ci.isCompleted, ci.animationObject, ci.requiredComponents);
                }

            }
            else if(prevActive && !isActive)
            {
                TargetManager.Instance.currentlyActive = null;
                TargetManager.Instance.canvas.enabled = false;
                uiManager.RemoveAllCheckItems();
            }
            prevActive = isActive;
        }
        
       
         
    }
}
