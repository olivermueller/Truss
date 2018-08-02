using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskModel {
    private static TaskModel instance;

    private TaskModel()
    {
        tasks = new List<Task>();
    }

    public static TaskModel Instance
    {
        get{
            if(instance == null){
                instance = new TaskModel();
            }
            return instance;
        }
    }
    
    public List<Task> tasks;
    
    public void AddTask(Task newTask)
    {
        tasks.Add(newTask);
    }

}
