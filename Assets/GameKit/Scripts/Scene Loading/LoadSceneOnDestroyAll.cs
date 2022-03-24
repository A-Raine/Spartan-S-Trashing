using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnDestroyAll : MonoBehaviour
{
	public GameObject[] entities;
	List<GameObject> entitiesList;
	[Space]
	public bool searchByTag;
	public string tagName;
	[Space]
	public string sceneToLoad;

	// Use this for initialization
	void Awake ()
	{
		if (searchByTag)
		{
			entities = GameObject.FindGameObjectsWithTag(tagName);
		}
		entitiesList = new List<GameObject>(entities);
		if (entitiesList.Count == 0)
		{
			Debug.Log("No entities found with this tag ! Turning off this component", gameObject);
			enabled = false;
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (entitiesList.Count == 0)
		{
			SceneMgr sceneManager = FindObjectOfType<SceneMgr>();
			if (sceneManager == null)
			{
				Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
			}
			else
			{
				sceneManager.LoadScene(sceneToLoad);
			}
			enabled = false;
		}
		else
		{
			entitiesList.RemoveAll(item => item == null);
		}
	}
}
