using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[System.Serializable]
public class Node : MonoBehaviour
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;
    public TaskData TaskData;
    public Action<Node> OnRemoveNode;

    public void Initialize(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, TaskData TaskData)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoint = gameObject.AddComponent<ConnectionPoint>();
        inPoint.Initialize(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = gameObject.AddComponent<ConnectionPoint>();
        outPoint.Initialize(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
        this.TaskData = TaskData;
        title = GetType().FullName;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    private bool showTaskList = false;
    private int taskListHeight = 150;
    
    public virtual void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, "", style);
        GUI.Label(new Rect(rect.position.x+rect.width/2, rect.position.y +20, rect.width/2, 20), title);
        if (GUI.Button(new Rect(rect.position.x + 10, rect.position.y + rect.height - 70, rect.width - 20, 40),
            "Trainer Task List"))
        {
            print("pressed");
            showTaskList = !showTaskList;
        }

        if (showTaskList)
        {
            if (TaskData.tasks == null)
            {
                TaskData.tasks = new List<string>();
            }
            
            //make the height of the list box match the sizd of the task list
            taskListHeight = 60 * (TaskData.tasks.Count + 1);
            
            Rect taskListRect = new Rect(rect.position.x + 10, rect.position.y + rect.height + 150,
                rect.width - 20, taskListHeight);
            GUI.Box(taskListRect, "", style);
            if (GUI.Button(new Rect(taskListRect.position.x + 20, taskListRect.position.y + taskListRect.height - 50, taskListRect.width - 30, 40),"+"))
            {
                taskListHeight += 150;
                TaskData.tasks.Add("Add Task!");
            }

            for (int i = 0; i < TaskData.tasks.Count; i++)
            {
                DrawTextBox(i);
            }
        }
        
    }

    void DrawTextBox(int index)
    {
        TaskData.tasks[index] = EditorGUI.TextField(new Rect(rect.position.x + 30, rect.position.y + rect.height + 160 + index * 60,
            rect.width - 60, 40), "", TaskData.tasks[index]);
        if (GUI.Button(new Rect(rect.position.x + rect.width - 20, rect.position.y + rect.height + 150 + index * 60,
            40, 40), "-"))
        {
            TaskData.tasks.RemoveAt(index);
        }
    }

    public bool ProcessEvents(Event e, Vector2 mousePos, Vector2 drag)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(mousePos))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(mousePos))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(drag);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}

#endif