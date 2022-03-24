using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
	public bool stickOnlyOnTop;
	public bool useTag;
	public string tagName = "Case Sensitive";
	Transform colTransform;

	private void Awake ()
	{
		if(tagName == "Case Sensitive" && useTag)
		{
			useTag = false;
			Debug.Log("No tag set ! Setting useTag to false", gameObject);
		}
	}

	private void OnCollisionEnter (Collision collision)
	{
		if(useTag)
		{
			if (collision.gameObject.tag == tagName)
			{
				if(stickOnlyOnTop)
				{
					if(collision.transform.position.y > transform.position.y)
					{
						colTransform = collision.gameObject.transform;
						colTransform.parent = transform;
					}
				}
				else
				{
					colTransform = collision.gameObject.transform;
					colTransform.parent = transform;
				}

			}
		}
		else
		{
			if (stickOnlyOnTop)
			{
				if (collision.transform.position.y > transform.position.y)
				{
					colTransform = collision.gameObject.transform;
					colTransform.parent = transform;
				}
			}
			else
			{
				colTransform = collision.gameObject.transform;
				colTransform.parent = transform;
			}
		}
	}

	private void OnCollisionExit (Collision collision)
	{
		//Debug.Log("Leaving ! Collision name : " + collision.gameObject.name);
		if (useTag)
		{
			if (collision.gameObject.tag == tagName)
			{
				colTransform = collision.gameObject.transform;
				colTransform.parent = null;
			}
		}
		else
		{
			colTransform = collision.gameObject.transform;
			colTransform.parent = null;
		}
	}
}
