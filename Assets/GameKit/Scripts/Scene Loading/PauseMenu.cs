using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	[SerializeField] string pauseInputName = "Cancel";
	[SerializeField] GameObject pauseMenu = null;
	[SerializeField] GameObject ResumeButton = null;
	SceneMgr sceneManager;

	EventSystem es;
	bool isPaused = false;
	float previousTimeScale = 1f;

	void Awake ()
	{
		sceneManager = FindObjectOfType<SceneMgr>();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}

		es = FindObjectOfType<EventSystem>();
		if (es == null)
		{
			Debug.Log("No EventSystem found in the scene ! WTF", gameObject);
		}
	}

	private void Update ()
	{
		if(Input.GetButtonDown(pauseInputName) || Input.GetKeyDown(KeyCode.Escape))
		{
			if(isPaused)
			{
				pauseMenu.SetActive(false);
				isPaused = false;
			}
			else
			{
				pauseMenu.SetActive(true);
				es.SetSelectedGameObject(ResumeButton);
				isPaused = true;
			}
			SetTimeScale();
		}
	}


	void SetTimeScale()
	{
		if(isPaused)
		{
			previousTimeScale = Time.timeScale;
			Time.timeScale = .001f;
		}
		else
		{
			Time.timeScale = previousTimeScale;
		}
	}


	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		isPaused = false;
		SetTimeScale();
	}

	public void ReloadScene ()
	{
		pauseMenu.SetActive(false);
		isPaused = false;
		SetTimeScale();
		if (sceneManager == null)
		{
			Debug.Log("No Scene Manager found in the scene ! Add one to any object in the scene", gameObject);
		}
		else
		{
			string sceneToLoad = SceneManager.GetActiveScene().name;
			sceneManager.LoadScene(sceneToLoad);
		}
	}

	public void QuitGame ()
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
