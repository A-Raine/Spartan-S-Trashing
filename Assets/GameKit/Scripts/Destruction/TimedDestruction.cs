using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour
{
	[Tooltip("Time before destroying the object")]
	public float lifeTime = 1f;
	// Use this for initialization
	void Start ()
	{
		Destroy(gameObject, lifeTime);
	}
}
