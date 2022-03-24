using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DirectionalImpulse : MonoBehaviour
{
	public enum DirectionInputType { Axis, MousePos, Forward }
	public enum Axis { X, Y, Z, None }
	public enum AxisCheck { X,Y,Z}

	//[Header("Input")]
	public DirectionInputType directionInputType = DirectionInputType.Axis;

	public Axis horizontalAxis = Axis.X;
	[SerializeField] string horizontalAxisName = "Horizontal";

	public Axis verticalAxis = Axis.Y;
	[SerializeField] string verticalAxisName = "Vertical";

	[SerializeField] string impulseInputName = "Fire1";

	[Tooltip("Which axis is the depth ?")]
	[SerializeField] AxisCheck depthAxis = AxisCheck.Z;

	//[Header("Force")]
	[SerializeField] bool resetVelocityOnImpulse = false;
	[SerializeField] float impulseForce = 5f;

	//[Header("Cooldown")]
	public bool useCooldown = false;
	[SerializeField] float cooldown = 1f;

	//[Header("Collision Check")]
	[Tooltip("Minimal height required to perform an impulse. Helps preventing wall jumps while on the ground.")]
	public float minimalHeightToImpulse = 0;
	[Tooltip("Which axis is the depth ?")]
	[SerializeField] AxisCheck heightAxis = AxisCheck.Y;
	[SerializeField] LayerMask groundDetectionLayerMask = 1;
	[Tooltip("Offset from the pivot when detecting collision")]
	[SerializeField] Vector3 collisionOffset = new Vector3(0, 0.1f, 0);

	//[Header("Resources")]
	public bool requireResources = false;
	[SerializeField] ResourceManager resourceManager;
	[SerializeField] int resourceIndex = 0;
	[SerializeField] int resourceCostOnUse = 1;

	//[Header("Move")]
	[Tooltip("Do we prevent moving for some time after impulsion?")]
	public bool preventMovingAfterImpulse = true;
	[Tooltip("Reference to the Mover Component")]
	public Mover mover;
	[Tooltip("Duration of movement prevention")]
	[SerializeField] float movePreventionDuration = 0.75f;

	//[Header("FX")]
	[Tooltip("Visual/Sound FX Instantiated on jump")]
	public GameObject impulseFX = null;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] Vector3 FXOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] float timeBeforeDestroyFX = 3f;

	[Tooltip("Animator Component")]
	public Animator animator = null;
	[Tooltip("Name of the Trigger Parameter to activate")]
	[SerializeField] string dashTriggerName = "dash";

	[HideInInspector] public bool showInput = true;
	[HideInInspector] public bool showForce = false;
	[HideInInspector] public bool showCooldown = false;
	[HideInInspector] public bool showCollision = false;
	[HideInInspector] public bool showResources = false;
	[HideInInspector] public bool showMovement = false;
	[HideInInspector] public bool showFX = false;
	[HideInInspector] public bool showAnim = false;

	float timer = 0f;
	Rigidbody rigid;

	private void Awake ()
	{
		rigid = GetComponent<Rigidbody>();

		if(preventMovingAfterImpulse && mover == null)
		{
			mover = GetComponent<Mover>();
		}

		if(useCooldown)
		{
			timer = cooldown;
		}

		if(requireResources && resourceManager == null)
		{
			resourceManager = FindObjectOfType<ResourceManager>();
		}

		if(animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}

	bool CooldownCheck()
	{
		if(useCooldown)
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
		else
		{
			return true;
		}
	}

	bool HeightCheck ()
	{
		if (minimalHeightToImpulse > 0f)
		{
			Vector3 rayDir = Vector3.zero;
			switch (heightAxis)
			{
				case AxisCheck.X:
				rayDir = Vector3.left;
				break;

				case AxisCheck.Y:
				rayDir = Vector3.down;
				break;

				case AxisCheck.Z:
				rayDir = Vector3.back;
				break;
			}

			RaycastHit hit;
			if (Physics.Raycast(transform.position + collisionOffset, rayDir, out hit, minimalHeightToImpulse, groundDetectionLayerMask))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		else
		{
			return true;
		}
	}

	Vector3 GetMouseDirection ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			//Debug.Log(hit.point);
			Vector3 cursorPos = hit.point;
			Vector3 toCursor = cursorPos - transform.position;

			switch (depthAxis)
			{
				case AxisCheck.X:
				toCursor.x = 0;
				break;

				case AxisCheck.Y:
				toCursor.y = 0;
				break;

				case AxisCheck.Z:
				toCursor.z = 0;
				break;
			}

			toCursor.Normalize();
			//Debug.Log("To Mouse Cursor : " + toCursor);
			return toCursor;
		}

		else
		{
			return Vector3.zero;
		}
	}

	Vector3 GetInputDirection ()
	{
		Vector3 inputDir = Vector3.zero;

		switch (horizontalAxis)
		{
			case Axis.X:
			inputDir.x = Input.GetAxis(horizontalAxisName);
			break;

			case Axis.Y:
			inputDir.y = Input.GetAxis(horizontalAxisName);
			break;

			case Axis.Z:
			inputDir.z = Input.GetAxis(horizontalAxisName);
			break;
		}

		switch (verticalAxis)
		{
			case Axis.X:
			inputDir.x = Input.GetAxis(verticalAxisName);
			break;

			case Axis.Y:
			inputDir.y = Input.GetAxis(verticalAxisName);
			break;

			case Axis.Z:
			inputDir.z = Input.GetAxis(verticalAxisName);
			break;
		}

		inputDir.Normalize();
		//Debug.Log("Input Direction : " + inputDir);
		return inputDir;
	}

	void ResourceCheck(Vector3 direction)
	{
		if(direction != Vector3.zero)
		{
			if (requireResources)
			{
				if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
				{
					Impulsion(direction);
				}
			}
			else
			{
				Impulsion(direction);
			}
		}
	}

	void Impulsion (Vector3 direction)
	{
		if(direction != Vector3.zero || directionInputType == DirectionInputType.Forward)
		{
			if (resetVelocityOnImpulse)
			{
				rigid.velocity = Vector3.zero;
			}

			rigid.AddForce(direction * impulseForce, ForceMode.Impulse);

			if(useCooldown)
			{
				timer = cooldown;
			}

			if (preventMovingAfterImpulse)
			{
				if (mover != null)
				{
					StartCoroutine(PreventMovement());
				}
			}

			if (impulseFX != null)
			{
				GameObject fx = Instantiate(impulseFX, transform.position + FXOffset, Quaternion.identity);

				if (timeBeforeDestroyFX > 0)
				{
					Destroy(fx, timeBeforeDestroyFX);
				}
			}

			if(animator != null)
			{
				animator.SetTrigger(dashTriggerName);
			}
		}
	}

	public IEnumerator PreventMovement()
	{
		mover.canMove = false;

		yield return new WaitForSeconds(movePreventionDuration);

		mover.canMove = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if(CooldownCheck())
		{
			if (Input.GetButtonDown(impulseInputName))
			{
				if (HeightCheck())
				{
					if (directionInputType == DirectionInputType.Axis)
					{

						ResourceCheck(GetInputDirection());
					}
					else if (directionInputType == DirectionInputType.MousePos)
					{
						ResourceCheck(GetMouseDirection());
					}
					else
					{
						ResourceCheck(transform.forward);
					}
				}
			}
		}
	}
}
