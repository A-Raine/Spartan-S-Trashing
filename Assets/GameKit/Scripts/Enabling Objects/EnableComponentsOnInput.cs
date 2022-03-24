using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsOnInput : MonoBehaviour
{
	[Header("General")]
	public string inputName;
	public bool disableInstead;
	public Behaviour[] componentsToEnable;

	public enum RevertOn { InputUp, SecondPress, Timer, Never };
	[Header("Revert")]
	public RevertOn revertOn = RevertOn.SecondPress;

	public float revertAfterCooldown = 1f;

	bool wasActivated = false;

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

	public IEnumerator RevertAfterTime ()
	{
		yield return new WaitForSeconds(revertAfterCooldown);

		EnableComponents(false);
	}

	// Update is called once per frame
	void Update ()
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
					EnableComponents(true);
				}
			}
			else
			{
				EnableComponents(true);

				if (revertOn == RevertOn.Timer)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
			}

		}

		if (revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				EnableComponents(false);
			}
		}
	}
}
