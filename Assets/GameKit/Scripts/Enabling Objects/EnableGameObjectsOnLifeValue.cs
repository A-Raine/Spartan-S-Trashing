using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOnLifeValue : MonoBehaviour
{
	[SerializeField] bool disableInstead = false;

	[SerializeField] Life life;

	[SerializeField] GameObject[] gameObjects = new GameObject[1];

	[Space]

	[SerializeField] int lifeReachedToTrigger = 0;
	[SerializeField] bool enableIfLower = true;
	[SerializeField] bool triggerOnce = true;

	bool hasTriggered = false;

	// Use this for initialization
	void Start ()
	{
		if (life == null)
		{
			life = FindObjectOfType<Life>();
			Debug.LogWarning("No Life component set ! Finding one by default in the scene", gameObject);
		}
	}

	void UpdateComponents (bool state)
	{
		foreach (GameObject b in gameObjects)
		{
			b.SetActive(state);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (triggerOnce)
		{
			if (!hasTriggered)
			{
				if (enableIfLower && life.currentLife < lifeReachedToTrigger)
				{
					UpdateComponents(!disableInstead);
					hasTriggered = true;
				}

				if (!enableIfLower && life.currentLife > lifeReachedToTrigger)
				{
					UpdateComponents(!disableInstead);
					hasTriggered = true;
				}
			}

		}
		else
		{
			if (enableIfLower)
			{
				if (life.currentLife < lifeReachedToTrigger)
				{
					if (!hasTriggered)
					{
						UpdateComponents(!disableInstead);
						hasTriggered = true;
					}
				}
				else
				{
					if (hasTriggered)
					{
						UpdateComponents(disableInstead);
						hasTriggered = false;
					}
				}
			}
			else
			{
				if (life.currentLife > lifeReachedToTrigger)
				{
					if (!hasTriggered)
					{
						UpdateComponents(!disableInstead);
						hasTriggered = true;
					}
				}
				else
				{
					if (hasTriggered)
					{
						UpdateComponents(disableInstead);
						hasTriggered = false;
					}
				}
			}
		}
	}
}
