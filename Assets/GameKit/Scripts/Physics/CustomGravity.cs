using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
	//[Header("Gravity force options")]
	[Tooltip("The constant gravity force applied to the Rigidbody")]
	public Vector3 baseGravityForce = new Vector3(0f,-9.81f,0f);
	public Vector3 secondaryGravityForce = new Vector3(0f,9.81f,0f);
	[Tooltip("Maximum velocity of the Rigidbody")]
	public float maxVelocity = 50f;

	//[Space()]

	//[Header("Gravity inversion options")]
	[Tooltip("Do we invert gravity on Input ?")]
	public bool invertOnInput = true;
	[Tooltip("Input used for gravity inverting")]
	public string inputName = "Jump";
	[Tooltip("Is gravity change instant ?")]
	public bool instantGravityChangeOnInput = false;
	[Tooltip("Do we invert Jumping Direction ?")]
	public bool invertJumpingDirection = false;

	//[Space()]

	//[Header("Other options")]
	[Tooltip("Only Allow Gravity Inversion when grounded")]
	public bool onlyWhenGrounded = false;
	[Tooltip("Estimated distance between the pivot and the ground when Inverting")]
	public float collisionCheckDistance = 0.65f;
	[Tooltip("Do we flip the character when inverting gravity ?")]
	public bool InvertScaleOnGravityChange = true;
	public Transform transformToInvert;
	public Vector3 invertedRotation = new Vector3(-180, 0, 0);
	public Vector3 normalRotation = new Vector3(0, 0, 0);

	Rigidbody rigid;
	Jumper jump;

	bool isInverted = false;
	[HideInInspector] public Vector3 currentGravityForce;

	[HideInInspector] public bool showForces = true;
	[HideInInspector] public bool showGravity = true;
	[HideInInspector] public bool showGroundCheck = true;
	[HideInInspector] public bool showAnimation = true;
	// Use this for initialization
	void Start ()
	{
		currentGravityForce = baseGravityForce;

		rigid = GetComponent<Rigidbody>();
		if(rigid != null)
		{
			rigid.useGravity = false;
		}
		else
		{
			Debug.LogError("Add a Rigidbody without useGravity for this to work !", gameObject);
		}

		if(invertJumpingDirection)
		{
			jump = GetComponent<Jumper>();
			if(jump == null)
			{
				Debug.LogError("No Jumper Component found on this object !", gameObject);
				invertJumpingDirection = false;
			}
		}

		if(transformToInvert == null)
		{
			transformToInvert = transform;
		}
	}
	
	void ApplyGravity()
	{
		Vector3 baseGravity = baseGravityForce.normalized;
		baseGravity = Vector3.Scale(baseGravity, rigid.velocity);
		if(Mathf.Abs(baseGravity.magnitude) < maxVelocity)
		{
			rigid.AddForce(currentGravityForce, ForceMode.Acceleration);
		}
	}

	bool GroundCheck ()
	{
		RaycastHit hit;
		Vector3 dir = currentGravityForce.normalized;

		if (Physics.Raycast(transform.position, dir, out hit, collisionCheckDistance))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void InvertGravity()
	{
		if (Input.GetButtonDown(inputName) && rigid != null)
		{
			if(currentGravityForce == baseGravityForce)
			{
				currentGravityForce = secondaryGravityForce;
			}
			else
			{
				currentGravityForce = baseGravityForce;
			}
			
			if (instantGravityChangeOnInput)
			{
				rigid.velocity = Vector3.zero;
			}
			else
			{
				rigid.velocity = rigid.velocity * 0.9f;
			}

			if (invertJumpingDirection)
			{
				jump.jumpForce.y *= -1f;
			}

			isInverted = !isInverted;

			if(InvertScaleOnGravityChange)
			{
				if(isInverted)
				{
					transformToInvert.localEulerAngles = invertedRotation;
				}
				else
				{
					transformToInvert.localEulerAngles = normalRotation;
				}
			}
		}
	}

	void Update()
	{
		if (invertOnInput)
		{
			if(onlyWhenGrounded)
			{
				if(GroundCheck())
				{
					InvertGravity();
				}
			}
			else
			{
				InvertGravity();
			}	
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if(rigid != null)
		{
			ApplyGravity();
		}
	}
}
