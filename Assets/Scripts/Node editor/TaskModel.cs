﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Node_editor;
using UnityEditor;
using UnityEngine;

public class TaskModel:MonoBehaviour
{

    static TaskModel mInstance;
 
    public static TaskModel Instance
    {
        get
        {
            if (mInstance != null) return mInstance;
            mInstance = (TaskModel)FindObjectOfType(typeof(TaskModel));
            if (mInstance == null)
            {
                mInstance = (new GameObject("Task Model")).AddComponent<TaskModel>();
                Instance.nodeStyle = new GUIStyle();
                Instance.nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
                Instance.nodeStyle.border = new RectOffset(12, 12, 12, 12);

                Instance.selectedNodeStyle = new GUIStyle();
                Instance.selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
                Instance.selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

                Instance.inPointStyle = new GUIStyle();
                Instance.inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
                Instance.inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
                Instance.inPointStyle.border = new RectOffset(4, 4, 12, 12);

                Instance.outPointStyle = new GUIStyle();
                Instance.outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
                Instance.outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
                Instance.outPointStyle.border = new RectOffset(4, 4, 12, 12);
            }
            return mInstance;
        }
    }

    public List<TaskData> tasks;
    
    //Editor stuff
    public List<Node> nodes;
    public List<Connection> connections;

    public GUIStyle nodeStyle;
    public GUIStyle selectedNodeStyle;
    public GUIStyle inPointStyle;
    public GUIStyle outPointStyle;
    
    public void AddTask(TaskData newTaskData)
    {
        if(tasks == null) new List<TaskData>();
        tasks.Add(newTaskData);
        Debug.Log("Elements in task list: " + TaskModel.Instance.tasks.Count);

    }
}
