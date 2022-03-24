using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneMgr : MonoBehaviour
{
	public bool fadeBetweenScenes = false;
	public string fadeTriggerName = "Fade";
	public Animator fader;
	public Image fadeImage;

	[Header("Needed for any kind of scene loading")]
	string currentScene;
	[HideInInspector] public bool isLoadingScene = false;
	// Use this for initialization
	void Start ()
	{
		if(fadeBetweenScenes)
		{
			if (fader == null)
			{
				fader = GetComponentInChildren<Animator>();
			}

			if (fadeImage == null)
			{
				fadeImage = GetComponentInChildren<Image>();
			}
		}	
	}

	private void Awake ()
	{
		if(FindObjectsOfType<SceneMgr>().Length > 1)
		{
			Destroy(this);
		}
	}

	public void LoadScene(string sceneToLoad)
	{
		if(!isLoadingScene)
		{
			isLoadingScene = true;
			if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
			{
				if(fadeBetweenScenes)
				{
					StartCoroutine(Fading(sceneToLoad));
				}
				else
				{
					SceneManager.LoadScene(sceneToLoad);
				}
			
			}
		}
	}

	public IEnumerator Fading(string sceneToLoad)
	{
		fader.SetTrigger(fadeTriggerName);
		yield return new WaitUntil(() => fadeImage.color.a == 1);
		SceneManager.LoadScene(sceneToLoad);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
