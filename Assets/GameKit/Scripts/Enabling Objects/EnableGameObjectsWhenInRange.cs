using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsWhenInRange : MonoBehaviour
{
	public bool disableInstead = false;
	public float range;
	public Transform specificTarget;
	public string targetTagName;
	public GameObject[] gameObjectsToEnable;
	[Tooltip("Which layers do we want to affect ?")]
	public LayerMask layerMask = ~0;
	public bool revertWhenOutOfRange = true;

	bool isInRange = false;
	int targetsInRange = 0;
	float distanceToTarget;

	void ComponentManagement (bool isEnabled)
	{
		for (int i = 0; i < gameObjectsToEnable.Length; i++)
		{
			gameObjectsToEnable[i].SetActive(isEnabled);

		}
	}

	// Update is called once per frame
	void Update ()
	{
		targetsInRange = 0;
		if (specificTarget != null)
		{
			distanceToTarget = Vector3.Distance(transform.position, specificTarget.position);
			if (distanceToTarget <= range)
			{
				targetsInRange = 1;
			}
		}
		else
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
			if (hitColliders.Length >= 1)
			{
				for (int i = 0; i < hitColliders.Length; i++)
				{
					if (targetTagName != null)
					{
						if (hitColliders[i].tag == targetTagName)
						{
							targetsInRange++;
						}
					}
					else
					{
						targetsInRange++;
					}

				}
			}
		}

		if (targetsInRange > 0)
		{
			if (!isInRange)
			{
				ComponentManagement(!disableInstead);
				isInRange = true;
			}
		}
		else
		{
			if (isInRange)
			{
				if (revertWhenOutOfRange)
				{
					ComponentManagement(disableInstead);
				}
				isInRange = false;
			}
		}

	}
}
