using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnDestroy : MonoBehaviour
{
	[Tooltip("How many points we add or substract from the score ?")]
	public int scoreModif;
	public ScoreManager scoreMgr;
	// Use this for initialization
	private void Start ()
	{
		if(scoreMgr == null)
		{
			scoreMgr = FindObjectOfType<ScoreManager>();
		}
	}
	private void OnDestroy ()
	{
		scoreMgr.UpdateScore(scoreModif);
	}
}
