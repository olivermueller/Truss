using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class NestedTaskData : TaskData {
    
    public bool isFirst = true;
    private int subtaskId = 0;
    public TaskData iterator;
    public List<TaskData> _subTasks;
    public SubtaskEntry[] subTaskEntries;
    public int completedTasks;
    private bool completed;
    public NetworkedGameState gameState;
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
            gameState = FindObjectOfType<NetworkedGameState>();
            subTaskEntries = new SubtaskEntry [_subTasks.Count];
            for (int i = 0; i < _subTasks.Count; i++)
            {
                Debug.Log("Preparing: " + _subTasks[i]._title);
                _subTasks[i].parentTask = this;
                _subTasks[i].StartTask();
                subTaskEntries[i] = new SubtaskEntry() {Task = _subTasks[i], isCompleted = false};
            }
            GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
            GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
            isFirst = false;
        }
        
        if (completedTasks == _subTasks.Count && !completed)
        {
            completed = true;
            var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);
            
            gameState.nodeID = _out.ID;
            gameState.isDenied = true;
            
            player.CmdSetId(_out.ID);
            player.CmdResetBools();
            _out.StartTask();

            completed = false;
            completedTasks = 0;
            for (int i = 0; i < subTaskEntries.Length; i++)
            {
                subTaskEntries[i].isCompleted = false;
                subTaskEntries[i].isSelected = false;
            }
            
            return true;
        }
        foreach (var t in subTaskEntries)
        {
            if (t.isSelected) break;
            if (t.isCompleted || !t.Task.IsCompleted().HasValue || !t.Task.IsCompleted().Value) continue;
            Debug.Log("Found: " + t.Task._title + " in update");
            t.isSelected = true;
            iterator = t.Task;
            
            var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);

            gameState.nodeID = iterator.ID;
            gameState.isDenied = true;
            
            player.CmdSetId(iterator.ID);
            gameState.CmdSetDenied(true);
            
            print("----------------Subtask iterator ID: " + iterator.ID);
        }
        if (iterator == null) return null;
        // Evaluate if the subtask is completed
        var val = iterator.IsCompleted();
        if (val.HasValue)
        {
            if (val.Value)
            {
                // If it is complete, move to the next task
                //iterator = iterator.NextTask();
                //gameState.nodeID = iterator._out.ID;

               
                
                if (iterator == null || iterator as FinishTaskData)
                {
                    GameObject.FindGameObjectWithTag("CanvasTitle").GetComponent<TextMeshProUGUI>().text = _title;
                    GameObject.FindGameObjectWithTag("CanvasDescription").GetComponent<TextMeshProUGUI>().text = _description;
                    var subtask = subTaskEntries.First(t => t.isSelected);
                    print("Subtask: "+ subtask.Task._title + " Completed");
                    subtask.isCompleted = true;
                    subtask.isSelected = false;
                    if (subTaskEntries.All(t=>t.isCompleted))
                    {
                        return true;
                    }
                    
                    
            
                    var player = FindObjectsOfType<PlayerUnit>().First(p => p.isLocalPlayer);

                    gameState.nodeID = ID;
                    gameState.isDenied = true;
            
                    player.CmdSetId(ID);
                    gameState.CmdSetDenied(true);
                }
            }
            else
            {
                print("Is Completed: " + val.Value);
                try
                {
                    iterator = (iterator as AnswerTaskData).StartNoTask();
                }
                catch (Exception e)
                {
                    iterator = (iterator as AnswerTargetTaskData).StartNoTask();
                }
                
            }
        }
        // Subtask is not completed, return null.
        return null;
    }
}

[Serializable]
public class SubtaskEntry
{
    public TaskData Task;
    public bool isCompleted;
    public bool isSelected;
}
