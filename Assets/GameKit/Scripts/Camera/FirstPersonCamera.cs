using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
	[SerializeField] private Transform transformToRotateWithCamera;

	[Space]

	[Header("Mouse Parameters")]
	[SerializeField] private string mouseXInputName = "Mouse X";
	[SerializeField] private string mouseYInputName = "Mouse Y";
	[SerializeField] private float mouseSensitivityX = 250f;
	[SerializeField] private float mouseSensitivityY = 250f;

	[Space]

	[Header("Clamp Parameters")]
	[SerializeField] private float minXAxisClamp = -85f;
	[SerializeField] private float maxXAxisClamp = 85f;
	float xAxisClamp;
	// Use this for initialization
	private void Awake ()
	{
		LockCursor();
		xAxisClamp = 0;

		if(transformToRotateWithCamera == null)
		{
			Debug.LogWarning("No Player Body Set !", gameObject);
			transformToRotateWithCamera = GetComponentInParent<Mover>().transform;
		}
	}

	void LockCursor ()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void CameraRotation ()
	{
		float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivityX * Time.deltaTime;
		float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivityY * Time.deltaTime;

		xAxisClamp += mouseY;

		if(xAxisClamp > maxXAxisClamp)
		{
			xAxisClamp = maxXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(360f - maxXAxisClamp);
		}
		if (xAxisClamp < minXAxisClamp)
		{
			xAxisClamp = minXAxisClamp;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(minXAxisClamp * -1f);
		}

		transform.Rotate(Vector3.left * mouseY);
		if(transformToRotateWithCamera != null)
		{
			transformToRotateWithCamera.Rotate(Vector3.up * mouseX);	
		}
	}

	void ClampXAxisRotationToValue(float value)
	{
		Vector3 eulerRotation = transform.eulerAngles;
		eulerRotation.x = value;
		transform.eulerAngles = eulerRotation;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		CameraRotation();
	}
}
