using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

	public static void DrawLine(LineRenderer lr, Vector3 start, Vector3 end, float thickness,Color color)
	{
		lr.material = TaskModel.Instance.lineMaterial;
		lr.SetColors(color, color);
		lr.SetWidth(thickness, thickness);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

	}
}
