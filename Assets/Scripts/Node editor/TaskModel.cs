using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TaskModel:MonoBehaviour, ISerializationCallbackReceiver
{

    public List<Task> tasks;
    private static TaskModel _instance;

    public static TaskModel Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    public void AddTask(Task newTask)
    {
        if(tasks == null) new List<Task>();
        tasks.Add(newTask);
        Debug.Log("Elements in task list: " + TaskModel.Instance.tasks.Count);

    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        foreach (var task in tasks)
        {
            task.SetNextTask(tasks.First(t=>t.ID==task.nextID));
        }
    }
}
