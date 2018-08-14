using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


    public class NodeBasedEditor : EditorWindow
{
    

    private Vector2 offset;
    private Vector2 drag;

    private System.Type[] subnodes;
    private System.Type[] subtasks;
    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Node Based Editor");
    }

    private void OnEnable()
    {

//        var subnodes = ReflectiveEnumerator.GetEnumerableOfType<Node>();
//        Debug.Log("Amount of subclasses: " + subnodes.Count());
//        foreach (var node in subnodes)
//        {
//            Debug.Log(node.GetType().FullName);
//        }
        subnodes = TaskModel.GetSubClasses(typeof(Node));
        subnodes = subnodes.OrderBy(node => node.FullName).ToArray();
        Debug.Log("Amount of Node subclasses: " + subnodes.Count());
//        foreach (var node in subnodes)
//        {
//            Debug.Log(node);
//        }

        
        subtasks = TaskModel.GetSubClasses(typeof(TaskData));
        subtasks = subtasks.OrderBy(node => node.FullName).ToArray();
        Debug.Log("Amount of Task subclasses: " + subtasks.Length);
//        foreach (var node in subtasks)
//        {
//            Debug.Log(node.FullName);
//        }
        EditorApplication.playModeStateChanged += LogPlayModeState;
        for (int i = 0; i < subnodes.Length; i++)
        {
            Debug.Log("Node: " + subnodes[i] + " Task: " + subtasks[i]);
        }

        LogPlayModeState(PlayModeStateChange.EnteredEditMode);


    }

    private void LogPlayModeState(PlayModeStateChange state)
    {
        foreach (var connection in TaskModel.Instance.connections)
        {
            connection.OnClickRemoveConnection = OnClickRemoveConnection;
        }
        
        foreach (var node in TaskModel.Instance.nodes)
        {
            node.inPoint.OnClickConnectionPoint = OnClickInPoint;
            node.outPoint.OnClickConnectionPoint = OnClickOutPoint;
            node.OnRemoveNode = OnClickRemoveNode;
        }    
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
        if (TaskModel.Instance.selectedInPoint != null && TaskModel.Instance.selectedOutPoint == null)
        {
            Handles.DrawBezier(
                TaskModel.Instance.selectedInPoint.rect.center,
                e.mousePosition,
                TaskModel.Instance.selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (TaskModel.Instance.selectedOutPoint != null && TaskModel.Instance.selectedInPoint == null)
        {
            Handles.DrawBezier(
                TaskModel.Instance.selectedOutPoint.rect.center,
                e.mousePosition,
                TaskModel.Instance.selectedOutPoint.rect.center - Vector2.left * 50f,
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
        for (int i = 0; i < subnodes.Length; i++)
        {
            int index = i;
            genericMenu.AddItem(new GUIContent("Node/"+"Add "+subnodes[i].FullName), false, () => OnClickAddTargetNode(mousePosition,subnodes[index],subtasks[index]));
        }
        
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
            var nodeparent = GameObject.Find("Node Parent");
            if (nodeparent)
            {
                int childCount = nodeparent.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(nodeparent.transform.GetChild(i).gameObject);
                }
            }
        }

        if (TaskModel.Instance.connections!=null)
        {
            TaskModel.Instance.connections.Clear();
            var connectionparent = GameObject.Find("Connection Parent");
            if (connectionparent)
            {
                int childCount = connectionparent.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(connectionparent.transform.GetChild(i).gameObject);
                }
            }
        }

        if (TaskModel.Instance.tasks != null)
        {
            TaskModel.Instance.tasks.Clear();
            var taskparent = GameObject.Find("Task Parent");
            if (taskparent)
            {
                int childCount = taskparent.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(taskparent.transform.GetChild(i).gameObject);
                }
            }
        }
//        TaskModel.Instance.OnBeforeSerialize();
//        OnBeforeSerialize();
    }

    private void OnClickAddTargetNode(Vector2 mousePosition, Type nodetype, Type tasktype)
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
        var newTaskData = newTaskObj.AddComponent(tasktype) as TaskData;
        //newTaskData.Initialize(" ", " ", animationObj, targetObj);
        
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
            nodeparent.tag = "EditorOnly";
        }
        GameObject newNodeObj = new GameObject("Node Object");
        Node newNode = newNodeObj.AddComponent(nodetype) as Node;
        newNode.Initialize(mousePosition, 400, 300, TaskModel.Instance.nodeStyle, TaskModel.Instance.selectedNodeStyle, TaskModel.Instance.inPointStyle, TaskModel.Instance.outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, newTaskData);
        newNodeObj.transform.SetParent(nodeparent.transform);
        TaskModel.Instance.nodes.Add(newNode);
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        TaskModel.Instance.selectedInPoint = inPoint;

        if (TaskModel.Instance.selectedOutPoint != null)
        {
            if (TaskModel.Instance.selectedOutPoint.node != TaskModel.Instance.selectedInPoint.node)
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
        TaskModel.Instance.selectedOutPoint = outPoint;

        if (TaskModel.Instance.selectedInPoint != null)
        {
            if (TaskModel.Instance.selectedOutPoint.node != TaskModel.Instance.selectedInPoint.node)
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
                DestroyImmediate(connectionsToRemove[i].gameObject);
            }

            connectionsToRemove = null;
        }

        TaskModel.Instance.nodes.Remove(node);
        TaskModel.Instance.tasks.Remove(node.TaskData);
        DestroyImmediate(node.TaskData.gameObject);
        DestroyImmediate(node.gameObject);
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
        if (TaskModel.Instance.selectedOutPoint.type != ConnectionPointType.Nested)
        {
            TaskModel.Instance.selectedOutPoint.node.TaskData.SetNextTask(TaskModel.Instance.selectedInPoint.node
                .TaskData);
        }
        else
        {
            (TaskModel.Instance.selectedOutPoint.node.TaskData as NestedTaskData).AddSubTask(TaskModel.Instance.selectedInPoint.node.TaskData);
        }

        TaskModel.Instance.selectedInPoint.node.TaskData._prev = TaskModel.Instance.selectedOutPoint.node.TaskData;
        
        var connectionparent = GameObject.Find("Connection Parent");
        if (!connectionparent)
        {
            connectionparent = new GameObject("Connection Parent");
            connectionparent.tag = "EditorOnly";
        }
        var connectionGo = new GameObject("Connection Object");
        var connectioncomp = connectionGo.AddComponent<Connection>();
        connectioncomp.Initialize(TaskModel.Instance.selectedInPoint, TaskModel.Instance.selectedOutPoint, OnClickRemoveConnection);
        connectionGo.transform.SetParent(connectionparent.transform);
        TaskModel.Instance.connections.Add(connectioncomp);
    }//

    private void ClearConnectionSelection()
    {
        TaskModel.Instance.selectedInPoint = null;
        TaskModel.Instance.selectedOutPoint = null;
    }

    
}

public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
    {
        var objects = Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
            .Select(type => (T) Activator.CreateInstance(type, constructorArgs))
            .ToList();
//        objects.Sort();
        return objects;
    }
}