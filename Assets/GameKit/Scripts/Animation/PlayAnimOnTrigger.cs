using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnTrigger : MonoBehaviour
{
	public Animator animator = null;
	[SerializeField] string triggerName = "Case Sensitive";
	public bool useTag = false;
	[SerializeField] string tagName = "Case Sensitive";
	[SerializeField] bool triggerOnce = true;

	bool hasPlayed = false;

	private void OnTriggerEnter (Collider other)
	{
		if(triggerOnce == false || hasPlayed == false)
		{
			if (useTag)
			{
				if (tagName == other.tag)
				{
					if (animator != null)
					{
						animator.SetTrigger(triggerName);
						hasPlayed = true;
					}
					else
					{
						Debug.LogWarning("No animator set !",gameObject);
					}

				}
			}
			else
			{
				if (animator != null)
				{
					hasPlayed = true;
					animator.SetTrigger(triggerName);
				}
				else
				{
					Debug.LogWarning("No animator set !", gameObject);
				}
			}
		}
		
	}
}
