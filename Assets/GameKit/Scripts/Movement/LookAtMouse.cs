using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
	public enum AxisToUse { X, Y, Z };
	[Tooltip("Which axis do we use for rotation ?")]
	[SerializeField] AxisToUse axisToUse = AxisToUse.Y;
	[Tooltip("Do we use the Z Axis for rotation ?")]
	public Vector3 offset = Vector3.zero;

	

	void LookAtMouseCursor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			//Debug.Log(hit.point);
			Vector3 cursorPos = hit.point;

			switch (axisToUse)
			{
				case AxisToUse.X:
				cursorPos.x = transform.position.x;
				break;

				case AxisToUse.Y:
				cursorPos.y = transform.position.y;
				break;

				case AxisToUse.Z:
				cursorPos.z = transform.position.z;
				break;

			}

			cursorPos += offset;
			transform.LookAt(cursorPos);
		}
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		LookAtMouseCursor();
	}
}
