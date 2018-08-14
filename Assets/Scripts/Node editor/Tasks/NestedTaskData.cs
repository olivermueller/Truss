using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestedTaskData : TaskData {

    
    
    
    bool isFirst = true;
        private int subtaskId = 0;
        public TaskData iterator;
        public List<TaskData> _subTasks;
        public override void StartTask()
        {
            iterator = _subTasks[subtaskId];
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
                iterator = _subTasks[subtaskId];
                iterator.StartTask();
                isFirst = false;
            }
            // Evaluate if the subtask is completed
            var val = iterator.IsCompleted();
            if (val.HasValue && val.Value)
            {
                // If it is complete, move to the next task
                iterator = iterator.NextTask();
                if (iterator == null)
                {
                    Debug.Log("Subtask"+ subtaskId +" Completed");
                    // If there are no more tasks, move to the next subtask if there is any left
                    if (subtaskId < _subTasks.Count-1)
                    {
                        subtaskId++;
                        iterator = _subTasks[subtaskId];
                        iterator.StartTask();
                    }
                    // No more subtasks, subtasktask completed
                    else return true;
                }
            }
            // Subtask is not completed, return false.
            return false;
        }
}
