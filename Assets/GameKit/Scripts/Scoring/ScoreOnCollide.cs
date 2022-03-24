using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnCollide : MonoBehaviour
{

	[Tooltip("How many points we add or substract from the score ?")]
	public int scoreModif;
	[Tooltip("Do we change the score only once ?")]
	public bool onlyOnce;
	[Tooltip("Do we use a tag for collision ?")]
	public bool useTag;
	public string tagName;
	public ScoreManager scoreMgr;
	bool hasCollided = false;


	private void Start ()
	{
		if (scoreMgr == null)
		{
			scoreMgr = FindObjectOfType<ScoreManager>();
		}
	}

	private void OnCollisionEnter (Collision collision)
	{
		if (useTag)
		{
			if (tagName == collision.gameObject.tag)
			{
				if (onlyOnce)
				{
					if (hasCollided == false)
					{
						hasCollided = true;
						scoreMgr.UpdateScore(scoreModif);
					}
				}
				else
				{
					scoreMgr.UpdateScore(scoreModif);
				}

			}
		}
		else
		{
			if (onlyOnce)
			{
				if (hasCollided == false)
				{
					hasCollided = true;
					scoreMgr.UpdateScore(scoreModif);
				}
			}
			else
			{
				scoreMgr.UpdateScore(scoreModif);
			}
		}
	}
	private void OnTriggerEnter (Collider other)
	{
		if (useTag)
		{
			if (tagName == other.gameObject.tag)
			{
				if (onlyOnce)
				{
					if (hasCollided == false)
					{
						hasCollided = true;
						scoreMgr.UpdateScore(scoreModif);
					}
				}
				else
				{
					scoreMgr.UpdateScore(scoreModif);
				}

			}
		}
		else
		{
			if (onlyOnce)
			{
				if (hasCollided == false)
				{
					hasCollided = true;
					scoreMgr.UpdateScore(scoreModif);
				}
			}
			else
			{
				scoreMgr.UpdateScore(scoreModif);
			}
		}
	}
}
