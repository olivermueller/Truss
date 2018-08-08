using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Node_editor;

public class NodeBasedEditor : EditorWindow
{
    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;
    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Node Based Editor");
    }

    private void OnEnable()
    {
        
        
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (TaskModel.Instance.nodes != null)
        {
            for (int i = 0; i < TaskModel.Instance.nodes.Count; i++)
            {
                TaskModel.Instance.nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (TaskModel.Instance.connections != null)
        {
            for (int i = 0; i < TaskModel.Instance.connections.Count; i++)
            {
                TaskModel.Instance.connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (TaskModel.Instance.nodes != null)
        {
            for (int i = TaskModel.Instance.nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = TaskModel.Instance.nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add target task"), false, () => OnClickAddTargetNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Clear tasks"), false, () => ClearTaskList());
        
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (TaskModel.Instance.nodes != null)
        {
            for (int i = 0; i < TaskModel.Instance.nodes.Count; i++)
            {
                TaskModel.Instance.nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

   

    private void ClearTaskList()
    {
        if (TaskModel.Instance.nodes!=null)
        {
            TaskModel.Instance.nodes.Clear();
        }

        if (TaskModel.Instance.connections!=null)
        {
            TaskModel.Instance.connections.Clear();
        }
        TaskModel.Instance.tasks.Clear();
//        TaskModel.Instance.OnBeforeSerialize();
//        OnBeforeSerialize();
    }

    private void OnClickAddTargetNode(Vector2 mousePosition, string t = " ", string d = " ", GameObject targetObj = null,GameObject animationObj = null)
    {
        if (TaskModel.Instance.nodes == null)
        {
            TaskModel.Instance.nodes = new List<Node>();
        }
        if (TaskModel.Instance.tasks == null)
        {
            TaskModel.Instance.tasks = new List<TaskData>();
        }

        var taskparent = GameObject.Find("Task Parent");
        if (!taskparent)
        {
            taskparent = new GameObject("Task Parent");
        }
        GameObject newTaskObj = new GameObject("Task Object");
        TargetTaskData newTaskData = newTaskObj.AddComponent<TargetTaskData>();
        newTaskData.Initialize(" ", " ", animationObj, targetObj);
       // Debug.Log("TASK MODEL NAME" + TaskModel.Instance.name);
//        TargetTask newTask = new TargetTask(" ", " ", animationObj, targetObj);
       // Debug.Log("TASKS COUNT" + TaskModel.Instance.tasks.Count);
//        newTask.ID = TaskModel.Instance.tasks.Count;
        //
        newTaskObj.transform.SetParent(taskparent.transform);
        TaskModel.Instance.tasks.Add(newTaskData);
        var nodeparent = GameObject.Find("Node Parent");
        if (!nodeparent)
        {
            nodeparent = new GameObject("Node Parent");
        }
        GameObject newNodeObj = new GameObject("Node Object");
        TargetTaskNode newNode = newNodeObj.AddComponent<TargetTaskNode>();
        newNode.Initialize(mousePosition, 400, 300, TaskModel.Instance.nodeStyle, TaskModel.Instance.selectedNodeStyle, TaskModel.Instance.inPointStyle, TaskModel.Instance.outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, newTaskData);
        newNodeObj.transform.SetParent(nodeparent.transform);
        TaskModel.Instance.nodes.Add(newNode);
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        Debug.Log("Outpoint clicked");
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        if (TaskModel.Instance.connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < TaskModel.Instance.connections.Count; i++)
            {
                if (TaskModel.Instance.connections[i].inPoint == node.inPoint || TaskModel.Instance.connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(TaskModel.Instance.connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                TaskModel.Instance.connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        TaskModel.Instance.nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        TaskModel.Instance.connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (TaskModel.Instance.connections == null)
        {
            TaskModel.Instance.connections = new List<Connection>();
        }
        //Debug.Log("Ïn node:" + (selectedInPoint.node as TargetTaskNode)._targetTask._title + " ÖutNode: " + (selectedOutPoint.node as TargetTaskNode)._targetTask._title);
        
        // TODO Make it more abstract!
        (selectedOutPoint.node as TargetTaskNode).TargetTaskData.SetNextTask((selectedInPoint.node as TargetTaskNode).TargetTaskData);
        var connectionparent = GameObject.Find("Connection Parent");
        if (!connectionparent)
        {
            connectionparent = new GameObject("Connection Parent");
        }
        var connectionGo = new GameObject("Connection Object");
        var connectioncomp = connectionGo.AddComponent<Connection>();
        connectioncomp.Initialize(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);
        connectionGo.transform.SetParent(connectionparent.transform);
        TaskModel.Instance.connections.Add(connectioncomp);
    }//

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    
}