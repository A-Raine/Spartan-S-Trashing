using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
	[SerializeField] Transform teleportPosition = null;
	[SerializeField] Vector3 offset = Vector3.zero;
	[SerializeField] bool useTag = false;
	[SerializeField] string tagName = "Case Sensitive";

	[SerializeField] bool requireInput = false;
	[SerializeField] string inputName = "Case Sensitive";

	private void Awake ()
	{
		if (tagName == "Case Sensitive" && useTag)
		{
			useTag = false;
			Debug.Log("No tag set ! Setting useTag to false", gameObject);
		}

		if (inputName == "Case Sensitive" && requireInput)
		{
			requireInput = false;
			Debug.Log("No Input set ! Setting requireInput to false", gameObject);
		}
	}

	void Teleport(Collider other)
	{
		Rigidbody r = other.GetComponent<Rigidbody>();
		if (r == null)
		{
			r = other.GetComponentInParent<Rigidbody>();
		}

		if (useTag)
		{
			if (tagName == r.tag)
			{
				r.transform.position = teleportPosition.position + offset;
			}
		}
		else
		{
			r.transform.position = teleportPosition.position + offset;
		}
	}
	private void OnTriggerEnter (Collider other)
	{
		if(!requireInput)
		{
			Teleport(other);
		}
	}

	private void OnTriggerStay (Collider other)
	{
		if(requireInput)
		{
			if(Input.GetButtonDown(inputName))
			{
				Teleport(other);
			}
		}
	}
}
