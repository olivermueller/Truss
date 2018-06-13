using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public List<TargetElement> targets;

    public GameObject currentlyActive;
    public Canvas canvas;

    private static TargetManager _instance;
    public static TargetManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            targets = new List<TargetElement>();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("CylinderTarget"))
                targets.Add(g.GetComponent<TargetElement>());
            canvas = GameObject.FindObjectOfType<Canvas>();
        }
    }

    
}
