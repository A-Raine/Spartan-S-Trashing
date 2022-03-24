using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
	//[Header("Forces")]
	[Tooltip("Force and direction of the propulsion")]
	public Vector3 bumpForce = new Vector3(0f, 300f, 0f);
	[Tooltip("Do we add an additional force towards the colliding object ?")]
	public bool bumpTowardsOther = false;
	[Tooltip("Additional force added towards the colliding object ?")]
	[SerializeField] float additionalForceTowardsOther = 500f;
	[SerializeField] bool preventInputHolding = false;

	//[Header("Tag")]
	[Tooltip("Do we bump only objects with a specific tag ?")]
	public bool useTag = false;
	[Tooltip("Name of the tag used on collision")]
	public string tagName = "Case sensitive";

	private void Awake ()
	{
		if (tagName == "Case Sensitive" && useTag)
		{
			useTag = false;
			Debug.Log("No tag set ! Setting useTag to false", gameObject);
		}
	}
	private void OnCollisionEnter (Collision collision)
	{
		Vector3 toOther = collision.transform.position - transform.position;
		if(collision.rigidbody != null)
		{
			if (useTag)
			{
				if (tagName == collision.gameObject.tag)
				{
					if(preventInputHolding)
					{
						Jumper j = collision.gameObject.GetComponent<Jumper>();

						if(j)
						{
							Debug.Log("AHHHH");
							j.isbeingBumped = true;
						}
					}

					collision.rigidbody.velocity = Vector3.zero;

					if (bumpTowardsOther)
					{
						collision.rigidbody.AddForce(toOther.normalized * additionalForceTowardsOther);
					}

					collision.rigidbody.AddForce(bumpForce);

				}
			}
			else
			{
				collision.rigidbody.velocity = Vector3.zero;

				if (bumpTowardsOther)
				{
					collision.rigidbody.AddForce(toOther.normalized * additionalForceTowardsOther);
				}

				collision.rigidbody.AddForce(bumpForce);
			}
		}
	}


	private void OnTriggerEnter (Collider other)
	{
		Vector3 toOther = other.transform.position - transform.position;
		if (useTag)
		{
			if (tagName == other.gameObject.tag)
			{
				Rigidbody otherRigid = other.GetComponent<Rigidbody>();
				if (otherRigid == null)
				{
					otherRigid = other.GetComponentInParent<Rigidbody>();
				}

				if (otherRigid != null)
				{
					if (preventInputHolding)
					{
						Jumper j = otherRigid.gameObject.GetComponent<Jumper>();

						if (j)
						{
							j.isbeingBumped = true;
						}
					}

					otherRigid.velocity = Vector3.zero;

					if (bumpTowardsOther)
					{
						otherRigid.AddForce(toOther.normalized * additionalForceTowardsOther + bumpForce);
					}

					otherRigid.AddForce(bumpForce);
				}
				else
				{
					Debug.LogWarning("No rigidbody found on colliding object ! Could not bump", gameObject);
				}
			}
		}
		else
		{
			Rigidbody otherRigid = other.GetComponent<Rigidbody>();
			if (otherRigid == null)
			{
				otherRigid = other.GetComponentInParent<Rigidbody>();
			}

			if (otherRigid != null)
			{
				otherRigid.velocity = Vector3.zero;

				if (bumpTowardsOther)
				{
					otherRigid.AddForce(toOther.normalized * additionalForceTowardsOther + bumpForce);
				}

				otherRigid.AddForce(bumpForce);
			}
			else
			{
				Debug.LogWarning("No rigidbody found on colliding object ! Could not bump", gameObject);
			}

		}
	}
}
