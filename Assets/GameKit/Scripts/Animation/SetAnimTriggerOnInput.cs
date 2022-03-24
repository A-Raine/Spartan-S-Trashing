using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimTriggerOnInput : MonoBehaviour
{
	[System.Serializable]
	public class TriggerInfo
	{
		public string triggerParameterName = "Trigger";
		public Animator animator;
	}

	[SerializeField] bool playOnce = false;
	[SerializeField] string inputName = "Fire1";

	[Space]

	public TriggerInfo[] triggers = new TriggerInfo[1];

	bool hasPressed = false;

	// Use this for initialization
	void Start ()
	{
		if(triggers.Length == 0)
		{
			//animators[0] = GetComponent<Animator>();
			Debug.LogWarning("No Trigger set !", gameObject);
		}
	}

	void InputCheck()
	{
		if (Input.GetButtonDown(inputName))
		{
			for (int i = 0; i < triggers.Length; i++)
			{
				triggers[i].animator.SetTrigger(triggers[i].triggerParameterName);
			}
			hasPressed = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(playOnce)
		{
			if(!hasPressed)
			{
				InputCheck();
			}
		}
		else
		{
			InputCheck();
		}
	}
}
