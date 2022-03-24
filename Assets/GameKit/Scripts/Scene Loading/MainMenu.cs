using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] string sceneToLoad = "Scene Name To Load";
	// Start is called before the first frame update
	SceneMgr sceneManager;
	private void Awake ()
	{
		sceneManager = FindObjectOfType<SceneMgr>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
	}

	public void LoadScene()
	{
		//SceneMgr sceneManager = FindObjectOfType<SceneMgr>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
		else
		{
			sceneManager.LoadScene(sceneToLoad);
		}
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
			{
				UnityEditor.EditorApplication.isPlaying = false;
			}
		#endif

		Application.Quit();
	}
}
