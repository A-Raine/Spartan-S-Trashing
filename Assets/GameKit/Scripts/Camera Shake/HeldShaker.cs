using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldShaker : MonoBehaviour
{
	[Header("Shake Values")]
	[SerializeField] float intensity = 0.5f;
	[SerializeField] float frequency = 0.05f;
	[SerializeField] float lerpSpeed = 0.05f;
	float timer = 0f;

	[SerializeField] Transform cam;

	Vector3 targetPos;
	Vector3 velocity;
	Vector3 initialPos;

	private void Awake ()
	{
		initialPos = cam.localPosition;
	}

	private void Update ()
	{
		if(Input.GetButton("Fire1"))
		{
			CamShake();
			cam.localPosition = Vector3.SmoothDamp(cam.localPosition, targetPos, ref velocity, lerpSpeed);
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			cam.localPosition = initialPos;
		}
	}
	public void CamShake ()
	{
		if (timer >= frequency)
		{
			Vector3 randomPoint = initialPos + Random.insideUnitSphere * intensity;

			targetPos = randomPoint;
			timer = 0f;
		}
		else
		{
			timer += Time.unscaledDeltaTime;
		}
	}
}
