using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
	[Tooltip("The life component we want to display")]
	public Life lifeToDisplay;
	Slider lifeBar;
	
	// Use this for initialization
	void Start ()
	{
		if (lifeToDisplay == null)
		{
			lifeToDisplay = FindObjectOfType<Life>();
			Debug.Log("LifeToDisplay n'a pas été assigné ! Pensez à drag & drop le component Life du GameObject dont vous voulez afficher la vie !", gameObject);
		}
		InitLifeBarValues();
	}

	public void InitLifeBarValues()
	{
		lifeBar = GetComponent<Slider>();
		lifeBar.minValue = 0;
		if (lifeToDisplay != null)
		{
			Debug.Log("Lifebar " + lifeBar.value);
			Debug.Log("Lifetodisplay : " + lifeToDisplay.currentLife);
			lifeBar.value = lifeToDisplay.currentLife;
			lifeBar.maxValue = lifeToDisplay.maxLife;
		}
	}

	public void UpdateValue()
	{
		if (lifeToDisplay != null)
		{
			lifeBar.value = lifeToDisplay.currentLife;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateValue();
	}
}
