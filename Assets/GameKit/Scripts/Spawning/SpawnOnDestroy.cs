using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
	public GameObject objectToSpawn;
	[SerializeField] bool spawnInsideSameParent = true;
	bool quitting = false;

	private void OnApplicationQuit ()
	{
		quitting = true;
	}

	private void OnDestroy ()
	{
		if(!quitting)
		{
			SceneMgr sceneManager = FindObjectOfType<SceneMgr>();
			if (sceneManager == null)
			{
				Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
				Instantiate(objectToSpawn, transform.position, Quaternion.identity);
			}
			else
			{
				if (!sceneManager.isLoadingScene)
				{
					GameObject g = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
					if(spawnInsideSameParent && transform.parent != null)
					{
						g.transform.parent = transform.parent;
					}
				}
			}
		}
	}
}
