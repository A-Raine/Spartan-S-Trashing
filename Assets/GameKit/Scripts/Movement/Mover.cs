using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour
{
	//[Header("Axis constraints")]
	public enum MovementType {Translation, Force}
	public MovementType movementType = MovementType.Force;
	public float maximumVelocity = 5f;
	[Tooltip("Do we move along the X Axis ?")]
	public bool useXAxis = true;
	[Tooltip("Do we move along the Y Axis ?")]
	public bool useYAxis = false;
	[Tooltip("Do we move along the Z Axis ?")]
	public bool useZAxis = false;

	//[Space(10f)]
	//[Header("Input")]
	[Tooltip("Do we need to press any input for the object to move ?")]
	public bool requiresInput = false;
	[Tooltip("Input required to move along X Axis")]
	public string xInputName;
	[Tooltip("Input required to move along Y Axis")]
	public string yInputName;
	[Tooltip("Input required to move along Z Axis")]
	public string zInputName;

	//[Space(10f)]
	//[Header("Movement")]
	[Tooltip("Move speed for each axis")]
	public Vector3 speed;

	[Tooltip("Does the object look at its movement direction ?")]
	public bool lookAtDirection = false;
	[Tooltip("Does the object move according to the local or world system")]
	public bool isMovementLocal = false;

	[Tooltip("Do we move against a wall ? Prevents being stuck on some surfaces")]
	public bool useWallAvoidance = false;
	[Tooltip("Bounds of wall detection")]
	public Vector3 wallAvoidanceSize = new Vector3(0.3f, 1f, 0.1f);
	[Tooltip("Local offset of wall detection")]
	public Vector3 wallAvoidanceOffset = new Vector3(0, 0, 0.5f);
	[Tooltip("Which Layers are considered a Wall ?")]
	public LayerMask wallAvoidanceLayers = 1;

	//[Space(10f)]
	//[Header("Animation")]
	[Tooltip("Reference to the Animator Component of the GameObject")]
	public Animator animator;

	public ParticleSystem movementParticleSystem;

	public float minVelocityToToggle = 0.2f;

	[Tooltip("Name of the float parameter used to animate movement on X axis")]
	public string xParameterName = "xSpeed";
	[Tooltip("Name of the float parameter used to animate movement on Y axis")]
	public string yParameterName = "ySpeed";
	[Tooltip("Name of the float parameter used to animate movement on Z axis")]
	public string zParameterName = "zSpeed";

	[Tooltip("Do we keep the previous orientation in memory if the GameObject doesn't move ?")]
	public bool keepOrientationOnIdle = true;

	[Tooltip("Do we send to the Animator the current speed of the GameObject ?")]
	public bool trackSpeed = true;
	[Tooltip("Name of the float parameter used to animate movement depending on speed")]
	public string speedParameterName = "Speed";

	[Tooltip("Do we animate specifically when the GameObject is grounded ? Allows to split ground and airborne animations")]
	public bool animateWhenGroundedOnly;
	[Tooltip("Distance between the pivot and the surface on which the GameObject moves.")]
	public float collisionCheckRadius = 0.65f;
	[Tooltip("Décalage du check de collision par rapport au point de pivot")]
	[SerializeField] Vector3 collisionOffset = new Vector3(0, 0.1f, 0);
	[Tooltip("Name of the float parameter used to animate when grounded")]
	public string groundedParameterName = "isGrounded";
	public LayerMask groundLayerMask = 1;


	float speedValue = 0f;
	Vector3 a_speed;

	Rigidbody r;
	CustomGravity c;

	[HideInInspector] public bool canMove = true;
	[HideInInspector] public Vector3 currentSpeed = Vector3.zero;

	private void Awake ()
	{
		if (animator == null)
		{
			if ((animator = GetComponent<Animator>()) == null)
			{
				animateWhenGroundedOnly = false;
				keepOrientationOnIdle = false;
				trackSpeed = false;
			}
		}

		r = GetComponent<Rigidbody>();

		if(r == null && movementType == MovementType.Force)
		{
			Debug.LogError("To apply Force based movement, this gameObject requires a Rigidbody");
			r = gameObject.AddComponent<Rigidbody>();		
		}

		c = GetComponentInChildren<CustomGravity>();
	}
	private void Start ()
	{
		
	}

	void HandleParticleEmission()
	{
		if(movementParticleSystem != null)
		{
			Vector3 hvMagnitude;
			if (movementType == MovementType.Force)
			{
				hvMagnitude = Vector3.Scale(r.velocity, currentSpeed.normalized);
			}
			else
			{
				hvMagnitude = currentSpeed;
			}
			
			bool isMoving = hvMagnitude.magnitude >= minVelocityToToggle;
			if(isMoving)
			{
				if(!movementParticleSystem.isPlaying)
				{
					movementParticleSystem.Play();
				}
			}
			else if(movementParticleSystem.isPlaying)
			{
				movementParticleSystem.Stop();
			}
		}
	}

	void MoveObject()
	{
		currentSpeed = Vector3.zero;

		//If Requires Input, assigns current speed depending on the set axises
		if (requiresInput)
		{
			if (useXAxis)
			{
				float xMult = Input.GetAxis(xInputName);
				if(xMult != 0f)
				{
					currentSpeed.x = speed.x * xMult;
				}
			}
			if (useYAxis)
			{
				float yMult = Input.GetAxis(yInputName);
				if (yMult != 0f)
				{
					currentSpeed.y = speed.y * yMult;
				}
			}
			if (useZAxis)
			{
				float zMult = Input.GetAxis(zInputName);
				if (zMult != 0f)
				{
					currentSpeed.z = speed.z * zMult;
				}
			}
		}
		else
		{
			if (useXAxis)
			{
				currentSpeed.x = speed.x;
			}
			if (useYAxis)
			{
				currentSpeed.y = speed.y;
			}
			if (useZAxis)
			{
				currentSpeed.z = speed.z;
			}
		}

		if (lookAtDirection && currentSpeed != Vector3.zero)
		{
			transform.forward = currentSpeed.normalized;
		}
		Vector3 offsetPos = transform.position + wallAvoidanceOffset;

		if ((useWallAvoidance && !Helper.WallCheck(transform, wallAvoidanceOffset, wallAvoidanceSize, wallAvoidanceLayers)) || !useWallAvoidance)
		{

			if (animator != null)
			{
				CheckSpeed(currentSpeed);
			}

			if (isMovementLocal)
			{
				if(movementType == MovementType.Translation)
				{
					transform.Translate(currentSpeed * Time.deltaTime, Space.Self);
				}
				else
				{
					Vector3 movement = Vector3.zero;
					movement += transform.right * currentSpeed.x;
					movement += transform.up * currentSpeed.y;
					movement += transform.forward * currentSpeed.z;

					r.AddForce(movement * Time.fixedDeltaTime  * 1500f, ForceMode.Acceleration);

					Vector3 clampedVelocity = r.velocity;

					
					if (r.useGravity || c != null)
					{
						clampedVelocity.y = 0f;
						clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
						clampedVelocity.y = r.velocity.y;
					}
					else
					{
						clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
					}

					r.velocity = clampedVelocity;
					//r.velocity = currentSpeed * acceleration;
				}
			}
			else
			{
				if (movementType == MovementType.Translation)
				{
					transform.Translate(currentSpeed * Time.deltaTime, Space.World);
				}
				else
				{

					//r.velocity = currentSpeed * acceleration;
					r.AddForce(currentSpeed * Time.fixedDeltaTime * 1500f, ForceMode.Acceleration);

					Vector3 clampedVelocity = r.velocity;

					if(r.useGravity || c != null)
					{
						clampedVelocity.y = 0f;
						clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
						clampedVelocity.y = r.velocity.y;
					}
					else
					{
						clampedVelocity = Vector3.ClampMagnitude(clampedVelocity, maximumVelocity);
					}

					r.velocity = clampedVelocity;
				}
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if(canMove)
		{
			MoveObject();
			HandleParticleEmission();
			if (animator != null && animateWhenGroundedOnly)
			{
				//Debug.Log("Raycasting");
				if (Helper.GroundCheck(transform.position + collisionOffset, collisionCheckRadius, groundLayerMask))
				{
					animator.SetBool(groundedParameterName, true);
				}
				else
				{
					animator.SetBool(groundedParameterName, false);
				}
			}
		}
	}



	private void OnDrawGizmos ()
	{
		Gizmos.matrix = transform.localToWorldMatrix;

		if (useWallAvoidance)
		{
			if(Helper.WallCheck(transform, wallAvoidanceOffset, wallAvoidanceSize, wallAvoidanceLayers))
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.green;
			}
			
			Gizmos.DrawWireCube(wallAvoidanceOffset, wallAvoidanceSize * 2f);

		}

		if(animator != null && animateWhenGroundedOnly)
		{
			if(Helper.GroundCheck(transform.position + collisionOffset, collisionCheckRadius, groundLayerMask))
			{
				Gizmos.color = Color.blue;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawWireSphere(collisionOffset, collisionCheckRadius);
		}
	}

	#region Animation

	public void CheckSpeed (Vector3 currentSpeed)
	{	
			a_speed = currentSpeed;

			if (trackSpeed)
			{
				speedValue = currentSpeed.magnitude;
			}

			if (animator != null)
			{
				UpdateAnimatorValues();
			}
	}

	public void UpdateAnimatorValues ()
	{
		if (keepOrientationOnIdle)
		{
			if (a_speed != Vector3.zero)
			{
				if (useXAxis && speed.x != 0)
				{
					animator.SetFloat(xParameterName, a_speed.x / speed.x);
				}
				if (useYAxis && speed.y != 0)
				{
					animator.SetFloat(yParameterName, a_speed.y / speed.y);
				}
				if (useZAxis && speed.z != 0)
				{
					animator.SetFloat(zParameterName, a_speed.z / speed.z);
				}
			}
			else
			{
				if (useXAxis)
				{
					if (animator.GetFloat(xParameterName) > 0f)
					{
						animator.SetFloat(xParameterName, 0.1f);
					}
					else if (animator.GetFloat(xParameterName) < 0f)
					{
						animator.SetFloat(xParameterName, -0.1f);
					}
				}
				if (useYAxis)
				{
					if (animator.GetFloat(yParameterName) > 0f)
					{
						animator.SetFloat(yParameterName, 0.1f);
					}
					else if (animator.GetFloat(yParameterName) < 0f)
					{
						animator.SetFloat(yParameterName, -0.1f);
					}
				}
				if (useZAxis)
				{
					if (animator.GetFloat(zParameterName) > 0f)
					{
						animator.SetFloat(zParameterName, 0.1f);
					}
					else if (animator.GetFloat(zParameterName) < 0f)
					{
						animator.SetFloat(zParameterName, -0.1f);
					}
				}
			}
		}
		else
		{
			if (useXAxis && speed.x != 0)
			{
				animator.SetFloat(xParameterName, a_speed.x / speed.x);
			}
			if (useYAxis && speed.y != 0)
			{
				animator.SetFloat(yParameterName, a_speed.y / speed.y);
			}
			if (useZAxis && speed.z != 0)
			{
				animator.SetFloat(zParameterName, a_speed.z / speed.z);
			}
		}
		if (trackSpeed)
		{
			animator.SetFloat(speedParameterName, speedValue);
		}
	}
	#endregion
}
