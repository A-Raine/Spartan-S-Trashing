using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
	public static float Remap (this float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
	public static float CurvedLerp(this Vector2 minMaxValue, AnimationCurve curve, float t)
	{
		float curveEvaluate = curve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minMaxValue.x, minMaxValue.y, curveEvaluate);

		return lerpedValue;
	}

	public static bool GroundCheck (Vector3 from, float radius, LayerMask wallAvoidanceLayers)
	{
		if(Physics.CheckSphere(from, radius, wallAvoidanceLayers))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static bool BoxGroundCheck (Vector3 from, Vector3 halfExtents, Quaternion rotation, LayerMask wallAvoidanceLayers)
	{
		if (Physics.CheckBox(from, halfExtents, rotation, wallAvoidanceLayers))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static Vector3 WorldToLocalSpace(Transform t, Vector3 world)
	{
		Vector3 localPos = t.right * world.x + t.up * world.y + t.forward * world.z;

		return localPos;
	}

	public static bool WallCheck (Transform t, Vector3 offset, Vector3 size, LayerMask wallAvoidanceLayers)
	{
		Vector3 localOffset = t.right * offset.x + t.up * offset.y + t.forward * offset.z;

		Vector3 checkPos = t.position + localOffset;

		if (Physics.CheckBox(checkPos, size, t.rotation, wallAvoidanceLayers))
		{
			//Debug.Log("HIT");
			return true;
		}
		else
		{
			return false;
		}
	}
}
