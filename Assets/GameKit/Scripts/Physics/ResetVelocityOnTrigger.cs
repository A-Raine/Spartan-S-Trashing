using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetVelocityOnTrigger : MonoBehaviour
{
	[SerializeField] bool resetOwnVelocity = true;
	[SerializeField] bool resetOthersVelocity = false;
	[SerializeField] bool useTag = true;
	[SerializeField] string tagName = "Player";

	void ResetVelocity(Collider other)
	{
		if (resetOthersVelocity)
		{
			Rigidbody rigid = other.GetComponent<Rigidbody>();
			if (rigid != null)
			{
				rigid.velocity = Vector3.zero;
			}
		}
		if (resetOwnVelocity)
		{
			Rigidbody rigid = GetComponent<Rigidbody>();
			if (rigid != null)
			{
				rigid.velocity = Vector3.zero;
			}
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(useTag)
		{
			if(tagName == other.tag)
			{
				ResetVelocity(other);
			}
		}
		else
		{
			ResetVelocity(other);
		}

	}
}
