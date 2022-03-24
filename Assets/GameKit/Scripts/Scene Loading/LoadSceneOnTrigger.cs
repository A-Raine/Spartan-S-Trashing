using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad = "Scene Name In Assets (Case Sensitive)";
	[Tooltip("Do we use a tag for trigger detection ?")]
	public bool useTag = false;
	public string tagName = "Case Sensitive";
	// Use this for initialization
	void Start ()
	{
		if (sceneToLoad == "Scene Name In Assets (Case Sensitive)")
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
			Debug.Log("No scene to load, using current scene");
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(enabled)
		{
			if (useTag)
			{
				if(tagName == other.tag)
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
					sceneManager.LoadScene(sceneToLoad);
				}
			}
		}
		
		
	}
}
