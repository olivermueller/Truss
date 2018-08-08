using System;
using FullSerializer;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType { In, Out }
[System.Serializable]
public class ConnectionPoint : MonoBehaviour
{
    public Rect rect;

    public ConnectionPointType type;

    public Node node;

    public GUIStyle style;
    [fsProperty]
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }
//
    public void Initialize(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
//        Debug.Log(style.normal.background);
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
//                if (style.normal.background==null)
//                {
//                    style = new GUIStyle();
//                    style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
//                    style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
//                    style.border = new RectOffset(4, 4, 12, 12);
//                    Debug.Log("Null?: "+OnClickConnectionPoint);
//                }
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
//                if (style.normal.background==null)
//                {
//                    style = new GUIStyle();
//                    style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
//                    style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
//                    style.border = new RectOffset(4, 4, 12, 12);
//                }
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}