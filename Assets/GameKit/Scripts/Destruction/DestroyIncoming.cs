using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIncoming : MonoBehaviour
{
	public bool useTag;
	public string tagName;
	// Use this for initialization

	private void OnTriggerEnter (Collider other)
	{
		if(useTag)
		{
			if(other.tag == tagName)
			{
				Destroy(other.gameObject);
			}
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
