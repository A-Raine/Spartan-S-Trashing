using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnDestroy : MonoBehaviour
{
	[Header("Shaking parameters")]
	[Range(0f, 3f)]
	public float shakeDuration = 1f;
	[Range(0f, 3f)]
	public float intensity = 1f;

	public Shaker targetToShake;

	[SerializeField] SceneMgr sceneMgr;
	// Use this for initialization
	void Start ()
	{
		if(targetToShake == null)
		{
			targetToShake = FindObjectOfType<Shaker>();
		}

		if(sceneMgr == null)
		{
			sceneMgr = FindObjectOfType<SceneMgr>();
		}
	}

	private void OnDestroy ()
	{
		if (!sceneMgr.isLoadingScene)
		{
			targetToShake.Shake(shakeDuration, intensity);
		}

	}
}
