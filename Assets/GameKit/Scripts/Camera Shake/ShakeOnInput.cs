using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnInput : MonoBehaviour
{
	[Header("Shaking parameters")]
	[Range(0f, 3f)]
	[SerializeField] float shakeDuration = 1f;
	[Range(0f, 3f)]
	[SerializeField] float intensity = 0.5f;

	[SerializeField] string inputName = "";
	[SerializeField] bool useCooldown = true;
	[SerializeField] float cooldown = 1.0f;

	[SerializeField] bool onlyOnce = false;

	[SerializeField] Shaker targetToShake = null;

	bool hasShaken = false;

	float timer;
	// Use this for initialization
	void Start ()
	{
		if (targetToShake == null)
		{
			targetToShake = FindObjectOfType<Shaker>();
		}
	}

	bool CanShake()
	{
		if(timer > 0f)
		{
			timer -= Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(useCooldown)
		{
			if(CanShake())
			{
				if(onlyOnce)
				{
					if (!hasShaken)
					{
						if (Input.GetButton(inputName))
						{
							targetToShake.Shake(shakeDuration, intensity);
							timer = cooldown;
							hasShaken = true;
						}
					}
				}
				else
				{
					if (Input.GetButton(inputName))
					{
						targetToShake.Shake(shakeDuration, intensity);
						timer = cooldown;
					}
				}
			}
		}
		else
		{
			if (onlyOnce)
			{
				if (!hasShaken)
				{
					if (Input.GetButtonDown(inputName))
					{
						targetToShake.Shake(shakeDuration, intensity);
						hasShaken = true;
					}
				}
			}
			else
			{
				if (Input.GetButtonDown(inputName))
				{
					targetToShake.Shake(shakeDuration, intensity);
				}
			}
		}
	}
}
