using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnInput : MonoBehaviour
{
	[Header("General")]
	public string inputName;
	public bool disableInstead;
	public GameObject[] gameObjectsToEnable;

	public enum RevertOn {InputUp, SecondPress, Timer, Never };
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
				for (int i = 0; i < gameObjectsToEnable.Length; i++)
				{
					gameObjectsToEnable[i].SetActive(!enable);
				}
			}
			else
			{
				for (int i = 0; i < gameObjectsToEnable.Length; i++)
				{
					gameObjectsToEnable[i].SetActive(enable);
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
			if(revertOn == RevertOn.SecondPress)
			{
				if(wasActivated)
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

				if(revertOn == RevertOn.Timer)
				{
					StopAllCoroutines();
					StartCoroutine(RevertAfterTime());
				}
			}
			
		}

		if(revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				EnableComponents(false);
			}
		}
	}
}
