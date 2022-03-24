using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsWithTimer : MonoBehaviour
{
	[SerializeField] bool disableInstead = false;
	[SerializeField] float timeBeforeEnable = 3f;
	[SerializeField] bool disableAfter = true;
	[SerializeField] float timeBeforeDisable = 3f;
	[SerializeField] bool loop = true;
	[SerializeField] Behaviour[] componentsToEnable = new Behaviour[0];


	void ComponentManagement (bool isEnabled)
	{
		for (int i = 0; i < componentsToEnable.Length; i++)
		{
			componentsToEnable[i].enabled = isEnabled;
		}
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(EnableComponents());
	}

	public IEnumerator EnableComponents()
	{
		yield return new WaitForSeconds(timeBeforeEnable);
		ComponentManagement(!disableInstead);
		if(disableAfter)
		{
			StartCoroutine(DisableComponents());
		}
	}

	public IEnumerator DisableComponents()
	{
		yield return new WaitForSeconds(timeBeforeDisable);
		ComponentManagement(disableInstead);
		if (loop)
		{
			StartCoroutine(EnableComponents());
		}
	}
}
