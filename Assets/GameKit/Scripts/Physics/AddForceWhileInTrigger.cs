using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AddForceWhileInTrigger : MonoBehaviour
{
	[Header("Forces")]
	[SerializeField] Vector3 addedForce = Vector3.up;
	[SerializeField] bool isLocal = false;
	[SerializeField] bool overrideForce = false;
	[Header("Tag")]
	[SerializeField] bool useTag = false;
	[SerializeField] string checkTag = "Player";

	private void OnTriggerStay (Collider other)
	{
		if(useTag )
		{
			if(other.tag == checkTag)
			{
				ApplyForce(other);
			}
		}
		else
		{
			ApplyForce(other);
		}
	}

	void ApplyForce(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();

		if (rb)
		{
			Vector3 appliedForce = addedForce;
			if (isLocal)
			{
				appliedForce = transform.right * addedForce.x;
				appliedForce += transform.up * addedForce.y;
				appliedForce += transform.forward * addedForce.z;
			}

			if (overrideForce)
			{
				rb.AddForce(appliedForce, ForceMode.VelocityChange);
			}
			else
			{
				rb.AddForce(appliedForce, ForceMode.Force);
			}
		}
	}
}
