using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnScoreReach : MonoBehaviour
{
	public enum Compare { StrictlySuperior, StrictlyInferior, StrictlyEqual, SuperiorOrEqual, InferiorOrEqual}

	[SerializeField] ScoreManager scoreMgr;

	[SerializeField] int scoreToReach = 0;

	[SerializeField] Compare compareCheck = Compare.SuperiorOrEqual;

	[SerializeField] string sceneToLoad = "Scene Name In Assets (Case Sensitive)";

	private void Start ()
	{
		if(sceneToLoad == "Scene Name In Assets (Case Sensitive)")
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
			Debug.Log("No scene to load, using current scene", gameObject);
		}

		if(scoreMgr == null)
		{
			scoreMgr = GetComponent<ScoreManager>();
			if (scoreMgr == null)
			{
				scoreMgr = FindObjectOfType<ScoreManager>();
			}
		}
	}

	private void Update ()
	{
		if(scoreMgr)
		{
			CompareScore(scoreMgr.score);
		}
	}

	public void CompareScore(int currentScore)
	{
		switch (compareCheck)
		{
			case Compare.SuperiorOrEqual:

			if(currentScore >= scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.InferiorOrEqual:
			if (currentScore <= scoreToReach)
			{
				SceneLoading();
			}
			break;

			case Compare.StrictlyEqual:

			if (currentScore == scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.StrictlyInferior:

			if (currentScore < scoreToReach)
			{
				SceneLoading();
			}

			break;

			case Compare.StrictlySuperior:

			if (currentScore > scoreToReach)
			{
				SceneLoading();
			}

			break;
		}
	}

	void SceneLoading()
	{
		SceneMgr sceneManager = FindObjectOfType<SceneMgr>();

		if(sceneManager != null)
		{
			sceneManager.LoadScene(sceneToLoad);
		}
		else
		{
			Debug.LogError("No Scene Manager found in the scene ! Please add one from the GameKit Prefabs.", gameObject);
		}
	}
}
