using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentsOnDestroyAll : MonoBehaviour
{
	public GameObject[] gameObjectsToDetect;
	[Space]
	public Behaviour[] componentsToEnable;
	[SerializeField] bool disableInstead = false;
	[Space]
	public bool searchByTag = false;
	public string tagName;

	[HideInInspector] public List<GameObject> entitiesList;

	void Awake ()
	{
		if (searchByTag)
		{
			gameObjectsToDetect = GameObject.FindGameObjectsWithTag(tagName);
		}
		entitiesList = new List<GameObject>(gameObjectsToDetect);
		if (entitiesList.Count == 0)
		{
			Debug.Log("No entities found with this tag or inside 'Game Objects To Detect' ! Turning off this component", gameObject);
			enabled = false;
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (entitiesList.Count == 0)
		{
			int arrayLength = componentsToEnable.Length;
			if (arrayLength != 0)
			{
				for (int i = 0; i < arrayLength; i++)
				{
					componentsToEnable[i].enabled = !disableInstead;
				}
			}

			enabled = false;
		}
		else
		{
			entitiesList.RemoveAll(item => item == null);
		}
	}
}
