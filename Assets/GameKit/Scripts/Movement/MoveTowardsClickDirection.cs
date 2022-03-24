using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveTowardsClickDirection : MonoBehaviour
{
	[SerializeField] Camera cam;

	[Header("Targeting Parameters")]
	[SerializeField] bool lockOnX = false;
	[SerializeField] bool lockOnY = true;
	[SerializeField] bool lockOnZ = false;
	[SerializeField] LayerMask clickMask = ~0;
	[Header("Movement Parameters")]
	[SerializeField] float moveSpeed = 500f;
	[SerializeField] float maxSpeed = 50f;
	[SerializeField] bool useInertia = true;
	[SerializeField] bool keepVelocity = true;
	[SerializeField] bool displayDirection = true;
	[SerializeField] Rigidbody rigid;

	[SerializeField] LineRenderer lineRenderer;


	Vector3 cursorPos;
	bool isMoving = false;

	// Use this for initialization
	void Start ()
	{
		if(displayDirection && lineRenderer == null)
		{
			Debug.LogWarning("No Line Renderer set ! (Optional)", gameObject);
			lineRenderer = GetComponent<LineRenderer>();
		}
		
		if (cam == null)
		{
			Debug.LogWarning("No Camera set !", gameObject);

			cam = Camera.main;
		}

		if(rigid == null)
		{
			Debug.LogWarning("No Rigidbody set !", gameObject);
			rigid = GetComponent<Rigidbody>();
		}
	}

	void SetTargetPos ()
	{
		if (cam != null)
		{
			if (Input.GetMouseButton(0))
			{
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f, clickMask))
				{
					//Debug.Log(hit.point);
					cursorPos = hit.point;
					if (lockOnY)
					{
						cursorPos.y = transform.position.y;
					}
					else if (lockOnX)
					{
						cursorPos.x = transform.position.x;
					}
					else if (lockOnZ)
					{
						cursorPos.z = transform.position.z;
					}
					if(!useInertia)
					{
						if(keepVelocity)
						{
							rigid.velocity = (cursorPos - transform.position).normalized * rigid.velocity.magnitude;
						}
						else
						{
							rigid.velocity = Vector3.zero;
						}
					}
					transform.LookAt(cursorPos);

					isMoving = true;
				}
			}
			if (lineRenderer != null && displayDirection)
			{
				SetLineDir();
			}
		}

	}

	void MovetowardsTargetPos ()
	{
		if(rigid != null)
		{
			if(rigid.velocity.sqrMagnitude < maxSpeed)
			{
				rigid.AddForce(transform.forward * moveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
			}
			else
			{
				rigid.velocity = rigid.velocity.normalized * Mathf.Sqrt(maxSpeed);
			}
		}
		
	}

	void SetLineDir()
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.forward * 3f + transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		SetTargetPos();
	}

	private void FixedUpdate ()
	{
		if (isMoving)
		{
			MovetowardsTargetPos();
		}
	}
}
