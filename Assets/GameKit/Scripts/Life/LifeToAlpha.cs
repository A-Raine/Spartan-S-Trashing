using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeToAlpha : MonoBehaviour
{
	[SerializeField] Life lifeToTrack;
	[SerializeField] Image imageToTweak;
	[SerializeField] bool invert = false;
	float ratio;
	Color col;
	// Use this for initialization
	void Start ()
	{
		if(lifeToTrack == null)
		{
			Debug.LogWarning("No Life component referenced !", gameObject);
			lifeToTrack = FindObjectOfType<Life>();
		}
		if(imageToTweak == null)
		{
			Debug.LogWarning("No Image component referenced !", gameObject);
			if ((imageToTweak = GetComponent<Image>()) == null)
			{
				imageToTweak = GetComponentInChildren<Image>();
			}
		}

		col = imageToTweak.color;
		float ratio;
		if (invert)
		{
			ratio = (float)lifeToTrack.currentLife / lifeToTrack.maxLife;
		}
		else
		{
			ratio = 1f - (float)lifeToTrack.currentLife / lifeToTrack.maxLife;
		}

		col.a = 1f - ratio;
		imageToTweak.color = col;
	}



	// Update is called once per frame
	void Update ()
	{
		if(lifeToTrack != null)
		{
			col = imageToTweak.color;

			float ratio;
			if (invert)
			{
				ratio = (float)lifeToTrack.currentLife / lifeToTrack.maxLife;
			}
			else
			{
				ratio = 1f - (float)lifeToTrack.currentLife / lifeToTrack.maxLife;
			}

			col.a = Mathf.MoveTowards(col.a, 1f - ratio, 0.3f * Time.deltaTime);
			//Debug.Log("Ratio : " + ratio);

			imageToTweak.color = col;
		}
	}
}
