using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
	public AudioClip soundToPlay;
	public bool playOnInput;
	public string input;
	// Use this for initialization
	void Start ()
	{
		if(!playOnInput)
		{
			AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(playOnInput)
		{
			if(Input.GetButtonDown(input))
			{
				AudioSource.PlayClipAtPoint(soundToPlay, transform.position);

			}
		}
	}
}
