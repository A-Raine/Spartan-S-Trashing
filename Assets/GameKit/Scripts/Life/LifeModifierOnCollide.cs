using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeModifierOnCollide : MonoBehaviour
{
	[SerializeField] bool useTag = false;
	[SerializeField] string tagName = "Case Sensitive";
	public int lifeModif = 1;
	public bool destroyAfter = true;


	void ApplyLifeModification (Life life)
	{
		life.ModifLife(lifeModif);
		if (destroyAfter)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if (useTag)
		{
			Life life = other.gameObject.GetComponent<Life>();

			if (life == null)
			{
				life = other.GetComponentInParent<Life>();
			}


			if (life != null)
			{
				if (life.gameObject.tag == tagName)
				{
					ApplyLifeModification(life);
				}
			}
		}
		else
		{
			Life life = other.gameObject.GetComponent<Life>();

			if (life == null)
			{
				life = other.GetComponentInParent<Life>();
			}

			if (life != null)
			{
				if (life.gameObject.tag == tagName)
				{
					ApplyLifeModification(life);
				}
			}
		}

	}

	private void OnCollisionEnter (Collision collision)
	{
		Life life = collision.gameObject.GetComponent<Life>();

		if (life == null)
		{
			life = collision.gameObject.GetComponentInParent<Life>();
		}

		if (life != null)
		{
			ApplyLifeModification(life);
		}
	}
}
