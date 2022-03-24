using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
	[SerializeField] Camera cam;
	[Space]
	[Header("Lock options")]
	[SerializeField] bool lockOnX = false;
	[SerializeField] bool lockOnY = true;
	[SerializeField] bool lockOnZ = false;
	[Space]
	[Header("Movement options")]
	[SerializeField] float moveSpeed = 5f;
	[SerializeField] bool lookTowardsDestination = true;
	[Space]
	[Header("Clicking options")]
	[SerializeField] Vector3 offset = new Vector3(0f, 0.2f, 0f);
	[SerializeField] LayerMask clickMask = ~0;
	[SerializeField] GameObject clickMarker = null;

	[System.Serializable]
	public class AnimOptions
	{
		public Animator animator;
		public string isMovingParameterName = "Moving";
		public bool trackFacingDirection = false;
		public string hParameterName = "Horizontal";
		public string vParameterName = "Vertical";

	}
	[Space]
	[Header("Anim options")]
	public AnimOptions animOptions;

	GameObject currentClickMarker;
	ParticleSystem particles;

	Vector3 targetPos;
	bool isMoving = false;
	bool isStopped = true;

	// Use this for initialization
	void Start ()
	{
		if(clickMarker != null)
		{
			currentClickMarker = Instantiate(clickMarker, transform.position, clickMarker.transform.rotation);
			particles = currentClickMarker.GetComponent<ParticleSystem>();

			if(particles == null)
			{
				currentClickMarker.SetActive(false);
			}
			else
			{
				particles.Stop();
			}
		}
		Debug.Log(clickMask);

		if(cam == null)
		{
			cam = Camera.main;
		}

		if(animOptions.animator == null)
		{
			animOptions.animator = GetComponentInChildren<Animator>();
			Debug.LogWarning("No animator referenced !", gameObject);
		}
	}

	void UpdateAnimValues()
	{
		if(animOptions.animator != null)
		{
			animOptions.animator.SetBool(animOptions.isMovingParameterName, isMoving);

			if(animOptions.trackFacingDirection)
			{
				animOptions.animator.SetFloat(animOptions.hParameterName, transform.forward.x);
				animOptions.animator.SetFloat(animOptions.vParameterName, transform.forward.y);
			}
		}
	}

	void SetTargetPos()
	{
		if(cam!=null)
		{
			if(Input.GetMouseButton(0))
			{
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f, clickMask))
				{
					//Debug.Log(hit.point);
					Vector3 cursorPos = hit.point;
					if (lockOnY)
					{
						cursorPos.y = transform.position.y;
					}
					if (lockOnX)
					{
						cursorPos.x = transform.position.x;
					}
					if (lockOnZ)
					{
						cursorPos.z = transform.position.z;
					}

					if(Vector3.Distance(cursorPos, transform.position) > 1f)
					{
						if(currentClickMarker != null)
						{
							currentClickMarker.transform.position = cursorPos + offset;
							if (particles == null)
							{
								currentClickMarker.SetActive(false);
								currentClickMarker.SetActive(true);
							}
							else
							{
								if(isStopped == true)
								{
									particles.Play();
									isStopped = false;
								}
							}
						}

						targetPos = cursorPos;
						isMoving = true;
					}
				}
			}
		}	
	}

	void MovetowardsTargetPos()
	{
		if(Vector3.Distance(transform.position, targetPos) > 0.1f)
		{
			Vector3 movement = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
			transform.position = movement;

			if (lookTowardsDestination)
			{
				Vector3 cursorPos = targetPos;
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

				transform.LookAt(cursorPos);
			}
		}
		else
		{
			isMoving = false;
			if (currentClickMarker != null)
			{
				if (particles == null)
				{
					currentClickMarker.SetActive(false);
				}
				else
				{
					particles.Stop();
					isStopped = true;
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		//(ceDebug.Log(transform.forward);
		SetTargetPos();
		UpdateAnimValues();
	}

	private void FixedUpdate ()
	{
		if (isMoving)
		{
			MovetowardsTargetPos();
		}
	}
}
