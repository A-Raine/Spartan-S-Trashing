using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsOnTrigger : MonoBehaviour
{
	public enum RevertOn { InputUp, SecondPress, TriggerExit, Timer, Never }
	public Behaviour[] componentsToEnable = new Behaviour[1];

	[Header("Tag")]
	public bool useTag = false;
	public string tagName = "Case Sensitive";

	[Header("Settings")]
	public bool disableInstead = false;
	public RevertOn revertOn = RevertOn.TriggerExit;

	public float revertAfterCooldown = 1f;
	//public bool revertOnTriggerExit = false;

	[Header("Resources")]
	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;

	[Header("Input")]
	public bool requireInput = false;
	//public bool revertOnSecondInputPress = false;
	public string inputName = "Case Sensitive";

	bool wasActivated = false;

	bool isInside = false;

	private void Update ()
	{
		if (revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				if (wasActivated)
				{
					EnableComponents(false);
				}
			}
		}

		if (isInside)
		{
			if (requireInput)
			{
				InputCheck();
			}
		}
	}

	void EnableComponents (bool enable)
	{
		if (wasActivated == !enable)
		{
			if (disableInstead)
			{
				for (int i = 0; i < componentsToEnable.Length; i++)
				{
					componentsToEnable[i].enabled = !enable;
				}
			}
			else
			{
				for (int i = 0; i < componentsToEnable.Length; i++)
				{
					componentsToEnable[i].enabled = enable;
				}
			}
		}
		wasActivated = enable;
	}
	void ResourceCheck ()
	{
		if (requireResources)
		{
			if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
			{
				if (revertOn == RevertOn.Timer && !wasActivated)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
				EnableComponents(true);
			}
		}
		else
		{
			if (revertOn == RevertOn.Timer && !wasActivated)
			{
				StopAllCoroutines();
				StartCoroutine(RevertAfterTime());
			}
			EnableComponents(true);
		}
	}

	public IEnumerator RevertAfterTime ()
	{
		yield return new WaitForSeconds(revertAfterCooldown);

		EnableComponents(false);
	}
	private void OnTriggerStay (Collider other)
	{
		/*if (requireInput)
		{
			Debug.Log("TAGCHECK");
			if (useTag)
			{
				Rigidbody r = other.GetComponent<Rigidbody>();
				if (r == null)
				{
					r = other.GetComponentInParent<Rigidbody>();
				}

				if (r != null && tagName == r.tag)
				{
					Debug.Log("TAGCHECK B");
					InputCheck();
				}
			}
			else
			{
				InputCheck();
			}
		}*/
	}

	void InputCheck ()
	{
		if (Input.GetButtonDown(inputName))
		{
			if (revertOn == RevertOn.SecondPress)
			{
				if (wasActivated)
				{
					EnableComponents(false);
				}
				else
				{
					ResourceCheck();
				}
			}
			else
			{
				ResourceCheck();
			}
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (useTag)
		{
			Rigidbody r = other.GetComponent<Rigidbody>();
			if (r == null)
			{
				r = other.GetComponentInParent<Rigidbody>();
			}

			if (r != null)
			{
				if (tagName == r.tag)
				{
					isInside = true;
					if (!requireInput)
					{
						ResourceCheck();
					}
				}
			}
		}
		else
		{
			isInside = true;
			if (!requireInput)
			{
				ResourceCheck();
			}
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (revertOn == RevertOn.TriggerExit)
		{
			if (useTag)
			{
				Rigidbody r = other.GetComponent<Rigidbody>();
				if (r == null)
				{
					r = other.GetComponentInParent<Rigidbody>();
				}
				if (tagName == r.tag)
				{
					EnableComponents(false);
				}
			}
			else
			{
				EnableComponents(false);
			}
		}
		else
		{
			isInside = false;
		}
	}
}
