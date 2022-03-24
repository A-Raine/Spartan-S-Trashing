using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintPosition : MonoBehaviour
{

	[Tooltip("Do we constraint position on the X Axis ?")]
	public bool constraintX;
	[Tooltip("Do we constraint position on the Y Axis ?")]
	public bool constraintY;
	[Tooltip("Do we constraint position on the Z Axis ?")]
	public bool constraintZ;

	[Tooltip("Minimal coordinates we can reach for each axis")]
	public Vector3 minPos;
	[Tooltip("Maximal coordinates we can reach for each axis")]
	public Vector3 maxPos;

	Vector3 currentPos;

	void LimitXPos ()
	{
		if (transform.position.x < minPos.x)
		{
			currentPos.x = minPos.x;
		}
		else if (transform.position.x > maxPos.x)
		{
			currentPos.x = maxPos.x;
		}
	}

	void LimitYPos ()
	{
		if (transform.position.y < minPos.y)
		{
			currentPos.y = minPos.y;
		}
		else if (transform.position.y > maxPos.y)
		{
			currentPos.y = maxPos.y;
		}
	}

	void LimitZPos ()
	{
		if (transform.position.z < minPos.z)
		{
			currentPos.z = minPos.z;
		}
		else if (transform.position.z > maxPos.z)
		{
			currentPos.z = maxPos.z;
		}
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		currentPos = transform.position;
		if (constraintX)
		{
			LimitXPos();
		}
		if (constraintY)
		{
			LimitYPos();
		}
		if (constraintZ)
		{
			LimitZPos();
		}
		transform.position = currentPos;
	}
}
