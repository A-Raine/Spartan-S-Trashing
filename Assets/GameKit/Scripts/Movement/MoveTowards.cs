using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
	[Tooltip("The transform this object should move towards")]
	public Transform target;
	[Tooltip("The target tag this object should look for")]
	public string targetTag = "Player";
	[Tooltip("Does the object look at its target ?")]
	public bool lookAtTarget;
	[Tooltip("Movement speed towards the target")]
	public float moveSpeed;
	[Tooltip("Minimal distance between target and this object")]
	public float minDistance = 0.2f;
	// Use this for initialization
	void Start ()
	{
		if(target == null)
		{
			//Debug.Log("No target assigned !");
			target = GameObject.FindGameObjectWithTag(targetTag).transform;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (target && Vector3.Distance(transform.position, target.position) > minDistance)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
			if(lookAtTarget)
			{
				transform.LookAt(target);
			}
		}
	}
}
