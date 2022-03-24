using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
	[Tooltip("Do we check the collided object's tag before destroying the object ?")]
	public bool useTag = true;
	[Tooltip("Tag we should use if useTag is set to true")]
	public string specificTag;

	private void OnCollisionEnter (Collision collision)
	{
		if(useTag)
		{
			if(collision.gameObject.tag == specificTag)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			Destroy(gameObject);
		}
		
	}
}
