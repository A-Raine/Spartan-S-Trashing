using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
	public int startLife = 10;
	public int maxLife = 10;

	public int currentLife = 10;

	public float invincibilityDuration = 0.25f;

	float invTimer = 0f;

	public Animator animator;
	[SerializeField] string hitParameterName = "Hit";

	private void Awake ()
	{
		currentLife = startLife;
		if(maxLife < startLife)
		{
			maxLife = startLife;
			//Debug.Log("Maximum life is below current life !");
		}

		if(animator == null)
		{
			animator = GetComponentInChildren<Animator>();
		}
		//Debug.Log("Current life : " + currentLife);
	}

	private void Update ()
	{
		if(invTimer < invincibilityDuration)
		{
			invTimer += Time.deltaTime;
		}
	}
	public void ModifLife(int lifeMod)
	{
		if(lifeMod < 0)
		{
			if(invTimer >= invincibilityDuration)
			{
				if(animator != null)
				{
					animator.SetTrigger(hitParameterName);
				}

				currentLife += lifeMod;

				invTimer = 0f;
			}
		}
		else
		{
			currentLife += lifeMod;
		}

		if(currentLife <= 0)
		{
			LoadSceneOnDestroy sceneOnDestroy = GetComponent<LoadSceneOnDestroy>();
			if (sceneOnDestroy != null)
			{
				sceneOnDestroy.LoadScene();
			}
			Destroy(gameObject);
		}
		else if(currentLife > maxLife)
		{
			currentLife = maxLife;
		}
	}

}
