using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnTrigger : MonoBehaviour
{
	[Header("Shaking parameters")]
	[Range(0f, 3f)]
	public float shakeDuration = 0.5f;
	[Range(0f, 3f)]
	public float intensity = 0.5f;

	public Shaker targetToShake;

	[Header("Trigger parameters")]
	[SerializeField] bool useTag = true;
	[SerializeField] string tagName = "Player";
	[SerializeField] bool triggerOnce = true;

	Vector3 initialPos;
	bool hasTriggered = false;
	// Use this for initialization

	private void Start ()
	{
		if(targetToShake == null)
		{
			//Debug.LogWarning("No Target to Shake set !");
			targetToShake = FindObjectOfType<Shaker>();
		}
	}
	private void OnTriggerEnter (Collider other)
	{
		if(useTag)
		{
			if(tagName == other.tag)
			{
				if(triggerOnce)
				{
					if(!hasTriggered)
					{
						targetToShake.Shake(shakeDuration, intensity);
						hasTriggered = true;
					}
				}
				else
				{
					targetToShake.Shake(shakeDuration, intensity);
				}
			}
		}
		else
		{
			if (triggerOnce)
			{
				if (!hasTriggered)
				{
					targetToShake.Shake(shakeDuration, intensity);
					hasTriggered = true;
				}
			}
			else
			{
				targetToShake.Shake(shakeDuration, intensity);
			}
		}
	}
}
