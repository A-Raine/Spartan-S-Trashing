using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jumper : MonoBehaviour
{
	//[Header("Jump")]
	[Tooltip("Input name used for jumping (From InputManager)")]
	[SerializeField] string jumpInputName = "Jump";
	[Tooltip("Force applied when jumping")]
	public Vector3 jumpForce = new Vector3(0f, 500f, 0f);
	[Tooltip("Which Layers are considered as ground ?")]
	[SerializeField] LayerMask jumpLayerMask = 1;

	[Range(0f, 10f)]
	[Tooltip("Under which velocity do we consider the jump to end ?")]
	[SerializeField] float jumpEndYVelocity = 1f;

	[Range(0f, 5f)]
	[Tooltip("Additional gravity multiplier applied after jump ended")]
	[SerializeField] float fallGravityMultiplier = 3f;
	
	[Range(0f, 5f)]
	[Tooltip("Additional gravity multiplier applied during jump if button was released")]
	[SerializeField] float lowJumpGravityMultiplier = 3f;

	//[Header("Air Jump")]
	[Tooltip("Amount of consecutive jumps allowed")]
	public int jumpAmount = 1;
	[Tooltip("Force applied when jumping")]
	public Vector3 airJumpForce = new Vector3(0f, 350f, 0f);
	[Tooltip("Do we reset the velocity on each consecutive jump ?")]
	public bool resetVelocityOnAirJump = false;

	//[Header("Collision Check")]
	public bool useCollisionCheck = true;
	[Tooltip("Offset from the pivot when detecting collision with the ground")]
	[SerializeField] Vector3 collisionOffset = new Vector3(0, 0.4f, 0);
	[Tooltip("Estimated width of the GameObject")]
	[SerializeField] float collisionCheckRadius = 0.5f;

	//[Header("FX")]
	[Tooltip("Visual/Sound FX Instantiated on jump")]
	public GameObject jumpFX = null;
	[Tooltip("Visual/Sound FX Instantiated on air jump")]
	public GameObject airJumpFX = null;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] Vector3 FXOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] float timeBeforeDestroyFX = 3f;


	//[Header("Animation")]
	[Tooltip("Reference to the Animator Component")]
	public Animator animator;
	[Tooltip("Name of the Trigger parameter called when jumping")]
	public string jumpTriggerName = "jump";
	[Tooltip("Name of the Trigger parameter called when air jumping. If you don't have any, set it to the value of jumpTriggerName")]
	public string airJumpTriggerName = "jump";

	[HideInInspector] public int currentJumpAmount;

	Rigidbody rigid;

	public bool isbeingBumped = false;

	CustomGravity c;
	public float characterHeight = 1.2f;
	WallJump wallJump;
	bool jumpRequest = false;

	// Use this for initialization
	void Start ()
	{
		wallJump = GetComponent<WallJump>();

		rigid = GetComponent<Rigidbody>();
		if ( rigid == null)
		{
			rigid = gameObject.AddComponent<Rigidbody>();
			Debug.Log("The GameObject doesn't have a Rigidbody !", gameObject);
		}

		if(animator == null)
		{
			animator = GetComponent<Animator>();
		}

		if(airJumpFX == null && jumpFX != null)
		{
			airJumpFX = jumpFX;
		}

		c = GetComponentInChildren<CustomGravity>();
		currentJumpAmount = jumpAmount;
	}

	bool GroundCheck ()
	{
		if (useCollisionCheck)
		{
			Vector3 checkPosition = transform.position + collisionOffset;
			if(c != null && c.currentGravityForce.y > 0)
			{
				checkPosition += Vector3.up * characterHeight;
			}
			if (Physics.CheckSphere(checkPosition, collisionCheckRadius, jumpLayerMask))
			{
				isbeingBumped = false;

				currentJumpAmount = jumpAmount;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return true;
		}
	}

	void GravityIncreases()
	{
		Vector3 gravity;
		if (c != null)
		{
			if(c.enabled)
			{
				gravity = c.currentGravityForce;
			}
			else
			{
				gravity = Physics.gravity;
			}

			if(gravity.y <= 0f)
			{
				if (rigid.velocity.y < jumpEndYVelocity)
				{
					rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
				}
				else if ((rigid.velocity.y > jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isbeingBumped)
				{
					rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
				}
			}
			else
			{
				if (rigid.velocity.y > jumpEndYVelocity)
				{
					rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
				}
				else if ((rigid.velocity.y < jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isbeingBumped)
				{
					rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
				}
			}
		}
		else
		{
			gravity = Vector3.up * Physics.gravity.y;
			if (rigid.velocity.y < jumpEndYVelocity)
			{
				rigid.velocity += gravity * (fallGravityMultiplier - 1) * Time.deltaTime;
			}
			else if ((rigid.velocity.y > jumpEndYVelocity && !Input.GetButton(jumpInputName)) || isbeingBumped)
			{
				rigid.velocity += gravity * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
			}
		}

	}

	bool WallJumpCheck()
	{
		if(wallJump != null)
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position + collisionOffset, Vector3.down, out hit, wallJump.minimalHeightAllowedToWallJump, wallJump.wallJumpLayerMask))
			{
				return true;
			}
			else
			{
				if (Physics.Raycast(transform.position + wallJump.collisionOffset, transform.forward * -1f, out hit, 2f))
				{
					//Debug.Log("FALSE DAT ");
					return false;
				}
				else
				{
					//Debug.Log("TRUE DAT ");
					return true;
				}
			}
		}
		else
		{
			return true;
		}
	}

	private void FixedUpdate ()
	{
		if(jumpRequest)
		{
			if (WallJumpCheck())
			{
				if (GroundCheck())
				{
					if (animator != null)
					{
						animator.SetTrigger(jumpTriggerName);
					}

					SpawnFX(jumpFX);
					Jump(jumpForce);
				}
				else if (jumpAmount > 1 && currentJumpAmount > 1)
				{
					if (animator != null)
					{
						animator.SetTrigger(airJumpTriggerName);
					}

					SpawnFX(airJumpFX);
					Jump(airJumpForce);

					currentJumpAmount--;
				}
			}
			jumpRequest = false;
		}

		GravityIncreases();
	}
	

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown(jumpInputName))
		{
			jumpRequest = true;
		}
			
	}

	void SpawnFX(GameObject spawnedFX)
	{
		if (spawnedFX != null)
		{
			GameObject fx = Instantiate(spawnedFX, transform.position + FXOffset, Quaternion.identity);

			if (timeBeforeDestroyFX > 0f)
			{
				Destroy(fx, timeBeforeDestroyFX);
			}
		}
	}

	void Jump(Vector3 appliedJumpForce)
	{
		if(resetVelocityOnAirJump)
		{
			rigid.velocity = Vector3.zero;
		}
		else
		{
			Vector3 velocity = rigid.velocity;
			velocity.y = 0;
			rigid.velocity = velocity;
		}

		appliedJumpForce = jumpForce;
		rigid.AddForce(appliedJumpForce);
	}

	private void OnCollisionEnter (Collision collision)
	{
		GroundCheck();
	}

	private void OnDrawGizmosSelected ()
	{
		//Debug.DrawLine(transform.position + collisionOffset, transform.position + collisionOffset - jumpForce.normalized * (collisionCheckDistance + collisionCheckDistance), Color.cyan);
		Vector3 checkPosition = transform.position + collisionOffset;
		if (c != null && c.currentGravityForce.y > 0)
		{
			checkPosition += Vector3.up * characterHeight;
		}

		if (Helper.GroundCheck(checkPosition, collisionCheckRadius, jumpLayerMask))
		{
			Gizmos.color = Color.blue;
		}
		else
		{
			Gizmos.color = Color.red;
		}
		Gizmos.DrawWireSphere(checkPosition, collisionCheckRadius);
	}
}
