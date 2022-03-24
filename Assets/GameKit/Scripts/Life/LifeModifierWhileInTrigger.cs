using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeModifierWhileInTrigger : MonoBehaviour
{
	[Header("Life")]
	[SerializeField] int lifeModif = 1;
	[SerializeField] float cooldown = 0.5f;
	[SerializeField] bool resetCooldownOnLeave = true;
	[Space]
	[Header("Tag")]
	[SerializeField] bool useTag = false;
	[SerializeField] string tagName = "Case Sensitive";

	float timer;

	private void Start ()
	{
		timer = cooldown;
	}

	bool CanDamage ()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return false;
		}
		else
		{
			return true;
		}
	}

	private void OnTriggerStay (Collider other)
	{
		Life lifeComponent = other.GetComponent<Life>();
		if (lifeComponent == null)
		{
			lifeComponent = other.GetComponentInParent<Life>();
		}

		if (lifeComponent != null)
		{
			if (useTag)
			{
				if (other.tag == tagName)
				{
					if (CanDamage())
					{
						lifeComponent.ModifLife(lifeModif);
						timer = cooldown;
					}
				}
			}
			else
			{
				if (CanDamage())
				{
					lifeComponent.ModifLife(lifeModif);
					timer = cooldown;
				}
			}
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if(resetCooldownOnLeave)
		{
			if(useTag)
			{
				if(other.tag == tagName)
				{
					timer = cooldown;
				}
			}
			else
			{
				timer = cooldown;
			}
		}
	}
}
