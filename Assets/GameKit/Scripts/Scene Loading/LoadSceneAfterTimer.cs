using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTimer : MonoBehaviour
{
	[Tooltip("The name of the scene we should load")]
	public string sceneToLoad = "Scene Name In Assets (Case Sensitive)";
	[Tooltip("Do we use a tag for trigger detection ?")]
	public float timeBeforeLoading = 2f;

	// Use this for initialization
	void Start ()
	{
		if (sceneToLoad == null)
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
			Debug.Log("No scene to load, using current scene", gameObject);
		}
		else
		{
			StartCoroutine(LoadSceneAfter());
		}
	}

	IEnumerator LoadSceneAfter()
	{
		yield return new WaitForSeconds(timeBeforeLoading);

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
