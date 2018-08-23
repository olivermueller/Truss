using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NestedTaskData : TaskData {

    
    
    
    bool isFirst = true;
    private int subtaskId = 0;
    public TaskData iterator;
    public List<TaskData> _subTasks;
    public SubtaskEntry[] subtasks;
    public override void StartTask()
    {
        iterator = null;
        base.StartTask();
    }
    public void AddSubTask(TaskData subtask)
    {
        if (_subTasks == null) _subTasks = new List<TaskData>();
        _subTasks.Add(subtask);
    }

    public override bool? IsCompleted()
    {
        // First time IsCompleted is called on SubTaskTask, set its local iterator to the first subtask.
        if(isFirst)
        {
            subtasks = new SubtaskEntry [_subTasks.Count];
            for (int i = 0; i < _subTasks.Count; i++)
            {
                Debug.Log("Preparing: " + _subTasks[i]._title);
                _subTasks[i].StartTask();
                subtasks[i] = new SubtaskEntry() {Task = _subTasks[i], isCompleted = false};
            }
            GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
            GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
            isFirst = false;
        }

        foreach (var t in subtasks)
        {
            if (t.isSelected) break;
            if (t.isCompleted || !t.Task.IsCompleted().HasValue || !t.Task.IsCompleted().Value) continue;
            Debug.Log("Found: " + t.Task._title + " in update");
            t.isSelected = true;
            iterator = t.Task;
        }
        if (iterator == null) return null;
        // Evaluate if the subtask is completed
        var val = iterator.IsCompleted();
        if (val.HasValue)
        {
            if (val.Value)
            {
                // If it is complete, move to the next task
                iterator = iterator.NextTask();
                if (iterator == null)
                {
                    GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
                    GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
                    var subtask = subtasks.First(t => t.isSelected);
                    subtask.isCompleted = true;
                    subtask.isSelected = false;
                    if (subtasks.All(t=>t.isCompleted))
                    {
                        return true;
                    }
                }
            }
            else
            {
                iterator = (iterator as AnswerTaskData).StartNoTask();
            }
        }
        // Subtask is not completed, return null.
        return null;
    }
}

public class SubtaskEntry
{
    public TaskData Task;
    public bool isCompleted;
    public bool isSelected;
}
