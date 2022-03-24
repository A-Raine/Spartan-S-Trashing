using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartingImpulse : MonoBehaviour
{
	public Vector3 impulsePower;
	public bool isLocal;

	Rigidbody rigid;
	// Use this for initialization
	void Start ()
	{
		rigid = GetComponent<Rigidbody>();

		if (isLocal)
		{
			Vector3 impulseDir = new Vector3(0, 0, 0);
			impulseDir += transform.up * impulsePower.y;
			impulseDir += transform.forward * impulsePower.z;
			impulseDir += transform.right * impulsePower.x;

			rigid.AddForce(impulseDir);
		}
		else
		{
			rigid.AddForce(impulsePower);
		}

	}

	// Update is called once per frame
	void Update ()
	{

	}
}
