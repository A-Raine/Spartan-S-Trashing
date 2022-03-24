using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAtPoint : MonoBehaviour
{
	public Vector3 spawnPosition;

	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad;
	[Tooltip("Do we use a tag for trigger detection ?")]
	public bool useTag;
	public string tagName;
	// Use this for initialization
	void Start ()
	{
		if (sceneToLoad == null)
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
			Debug.Log("No scene to load, using current scene", gameObject);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (enabled)
		{
			if (useTag)
			{
				if (tagName == other.tag)
				{
					SceneMgr sceneManager = FindObjectOfType<SceneMgr>();
					if (sceneManager == null)
					{
						Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
					}
					else
					{
						Life life = other.GetComponent<Life>();
						int lifeCount = 1;
						if (life != null)
						{
							lifeCount = life.currentLife;
						}

						PlayerSpawnData.UpdatePlayerSpawnPos(spawnPosition, lifeCount);
						sceneManager.LoadScene(sceneToLoad);
					}
				}
			}
			else
			{
				SceneMgr sceneManager = FindObjectOfType<SceneMgr>();
				if (sceneManager == null)
				{
					Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
				}
				else
				{
					Life life = other.GetComponent<Life>();
					int lifeCount = 1;
					if (life != null)
					{
						lifeCount = life.currentLife;
					}

					PlayerSpawnData.UpdatePlayerSpawnPos(spawnPosition, lifeCount);

					sceneManager.LoadScene(sceneToLoad);
				}
			}
		}


	}
}
