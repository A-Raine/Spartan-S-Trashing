using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOverTime : MonoBehaviour
{
	[Tooltip("Reference to the Score Manager Component")]
	[SerializeField] ScoreManager scoreManager;

	[Tooltip("How many points we add or substract from the score ?")]
	[SerializeField] int scoreModif = 1;

	[Range(0.1f, 10f)]
	[Tooltip("How often do we add or substract from the score ?")]
	[SerializeField] float scoreModifFrequency = 1f;

	float timer = 0;

	private void Awake ()
	{
		if(scoreManager == null)
		{
			scoreManager = FindObjectOfType<ScoreManager>();
		}

		if(scoreModif == 0)
		{
			Debug.LogWarning("Score modif value is equal to 0 !", gameObject);
		}

		timer = scoreModifFrequency;
	}

	// Update is called once per frame
	void Update()
    {
		timer -= Time.deltaTime;
		
		if(timer <= 0f)
		{
			if(scoreManager != null)
			{
				scoreManager.UpdateScore(scoreModif);
				timer = scoreModifFrequency;
			}
		}
    }
}
