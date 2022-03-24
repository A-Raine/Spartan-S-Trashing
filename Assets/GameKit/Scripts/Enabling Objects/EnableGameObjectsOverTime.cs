using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectsOverTime : MonoBehaviour
{
	[SerializeField] GameObject[] objects;
	[SerializeField] float cooldown = 0.2f;
	[SerializeField] bool disablePreviousOne = true;
	[SerializeField] bool loop = true;
	int index = 0;

	float timer = 0f;
	// Use this for initialization
	void Awake ()
	{
		if(objects.Length == 0 || objects == null)
		{
			int children = transform.childCount;
			objects = new GameObject[children];
			for (int i = 0; i < children; ++i)
			{
				objects[i] = transform.GetChild(i).gameObject;
			}
		}

		if(objects.Length != 0)
		{
			objects[0].SetActive(true);
		}
	}
	
	bool TimerCheck()
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
		if(TimerCheck() && objects.Length != 0)
		{
			if(index < objects.Length-1)
			{
				if(disablePreviousOne)
				{
					objects[index].SetActive(false);
				}
				index++;
				objects[index].SetActive(true);

				timer = cooldown;
			}
			else if(loop)
			{
				if (disablePreviousOne)
				{
					objects[index].SetActive(false);
				}
				index = 0;
				objects[index].SetActive(true);

				timer = cooldown;
			}
		}
	}
}
