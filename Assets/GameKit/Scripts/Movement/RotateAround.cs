using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
	public Transform target;
	public Vector3 rotateAxis;
	public float rotateSpeed;
	public bool useInput;
	public string inputName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(useInput)
		{
			
			float mvt = Input.GetAxis(inputName);
			if (mvt != 0f)
			{
				if (target)
				{
					transform.RotateAround(target.position, rotateAxis, rotateSpeed * Time.deltaTime * mvt);
				}
			}

		}
		else
		{
			if (target)
			{
				transform.RotateAround(target.position, rotateAxis, rotateSpeed * Time.deltaTime);
			}
		}

	}
}
