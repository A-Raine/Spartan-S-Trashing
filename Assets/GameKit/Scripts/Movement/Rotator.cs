using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	[Tooltip("Rotation speed on all 3 axis")]
	public Vector3 angularVelocity;
	[Tooltip("Do we need to press any input to rotate the object ?")]
	public bool rotateOnInput;
	[Tooltip("Name of the input used for rotating")]
	public string inputName;

	void RotateOnInput()
	{
		float angSpd = Input.GetAxis(inputName);
		if(angSpd != 0)
		{
			transform.Rotate(angularVelocity * angSpd * Time.deltaTime);
		}
	}

	void Rotate()
	{
		transform.Rotate(angularVelocity * Time.deltaTime);
	}

	// Update is called once per frame
	void Update ()
	{
		if(rotateOnInput)
		{
			RotateOnInput();
		}
		else
		{
			Rotate();
		}
	}
}
