using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsInRange : MonoBehaviour
{
	public float range;
	public Transform specificTarget;
	public string targetTagName;
	public bool disableInRange = false;
	public Behaviour[] components;
	[Tooltip("Which layers do we want to affect ?")]
	public LayerMask layerMask;
	public bool revertWhenOutOfRange = true;

	bool isInRange = false;
	int targetsInRange = 0;
	float distanceToTarget;
	// Use this for initialization
	void Start ()
	{

	}

	void ComponentManagement(bool isEnabled)
	{
		for(int i = 0; i < components.Length; i++)
		{
			components[i].enabled = isEnabled;	
		
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		targetsInRange = 0;
		if (specificTarget != null)
		{
			distanceToTarget = Vector3.Distance(transform.position, specificTarget.position);
			if(distanceToTarget <= range)
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
					if(targetTagName != null)
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

		if(targetsInRange > 0)
		{
			if(!isInRange)
			{
				ComponentManagement(!disableInRange);
				isInRange = true;
			}
		}
		else
		{
			if(isInRange)
			{
				if(revertWhenOutOfRange)
				{
					ComponentManagement(disableInRange);
				}
				isInRange = false;
			}
		}

	}
}
