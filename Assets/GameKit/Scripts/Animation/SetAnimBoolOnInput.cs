using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimBoolOnInput : MonoBehaviour
{
	[System.Serializable]
	public class BoolInfo
	{
		public string boolParameterName = "Case Sensitive";
		public bool valueSetOnInput = true;
		public Animator animator;
	}

	public enum RevertOn { InputUp, SecondPress, Never };
	[Header("Revert")]
	public RevertOn revertOn = RevertOn.SecondPress;

	[SerializeField] string inputName = "Fire1";

	[Space]

	public BoolInfo[] bools = new BoolInfo[1];

	bool wasActivated = false;

	// Use this for initialization
	void Start ()
	{
		if (bools.Length == 0)
		{
			//animators[0] = GetComponent<Animator>();
			Debug.LogWarning("No Bools set !", gameObject);
		}
	}

	void SetBool (bool toggle)
	{
		for (int i = 0; i < bools.Length; i++)
		{
			if (bools[i].valueSetOnInput)
			{
				bools[i].animator.SetBool(bools[i].boolParameterName, toggle);
			}
			else
			{
				bools[i].animator.SetBool(bools[i].boolParameterName, !toggle);
			}		
		}
		wasActivated = toggle;
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
					SetBool(false);
				}
				else
				{
					SetBool(true);
				}
			}
			else
			{
				SetBool(true);
			}

		}

		if (revertOn == RevertOn.InputUp)
		{
			if (Input.GetButtonUp(inputName))
			{
				SetBool(false);
			}
		}
	}
}
