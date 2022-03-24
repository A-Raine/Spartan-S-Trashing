using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
	public Transform target;
	public bool useTag;
	public string tagName;
	// Use this for initialization
	void Start ()
	{
		if (useTag && target == null)
		{
			target = GameObject.FindGameObjectWithTag(tagName).transform;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(target)
		{
			transform.LookAt(target);
		}
	}
}
