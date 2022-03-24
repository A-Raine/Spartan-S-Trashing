using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
	[SerializeField] float minZoom = 15f;
	[SerializeField] float maxZoom = 60f;
	[SerializeField] string zoomInputName = "Mouse ScrollWheel";
	[SerializeField] float sensitivity = 5000f;
	[SerializeField] float smoothSpeed = 50f;
	[SerializeField] Camera cam;

	float currentZoom;
	// Use this for initialization
	void Start ()
	{
		if(cam == null)
		{
			Debug.LogWarning("No camera set !", gameObject);
			cam = Camera.main;
		}
		currentZoom = cam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float zoomAxis = Input.GetAxis(zoomInputName);
		if (zoomAxis != 0f)
		{
			currentZoom -= zoomAxis * Time.deltaTime * sensitivity;
			currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
		}
		cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, currentZoom, smoothSpeed * Time.deltaTime);
	}
}
