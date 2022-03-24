using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallJump : MonoBehaviour
{
	//[Header("Wall Jump")]
	[Tooltip("Input name used for jumping (From InputManager)")]
	[SerializeField] string wallJumpInputName = "Jump";
	[Tooltip("Force applied when jumping")]
	public Vector3 wallJumpForce = new Vector3(0f, 250f, -350f);
	[Tooltip("Which Layers are considered as ground ?")]
	public LayerMask wallJumpLayerMask = 1;
	[Tooltip("Does the GameObject look towards the other way when wall jumping ? Very useful for consecutive wall jumps")]
	[SerializeField] bool invertLookDirectionOnWallJump = true;

	//[Header("Friction")]
	[Range(1, 10f)]
	[Tooltip("Slowdown effect applied when wall sliding. 1 = No effect")]
	[SerializeField] float gravityDampenWhileOnWall = 2f;

	//[Header("Move")]
	[Tooltip("Do we prevent moving for some time after wall Jumping ?")]
	public bool preventMovingAfterWallJump = true;
	[Tooltip("Duration of movement prevention")]
	[SerializeField] float movePreventionDuration = 0.25f;
	[Tooltip("Reference to the Mover Component")]
	[SerializeField] Mover mover;

	//[Header("Jump")]
	[Tooltip("Do we reset the jump count on Wall Jump ?")]
	public bool resetJumpCountOnWallJump = true;
	[Tooltip("Reference to the Jumper Component")]
	[SerializeField] Jumper jumper = null;

	//[Header("Collision Check")]
	[Tooltip("Estimated distance between the pivot and the wall when wall jumping")]
	public float collisionCheckDistance = 0.6f;
	[Tooltip("Minimal height required to perform a wall jump. Helps preventing wall jumps while on the ground.")]
	public float minimalHeightAllowedToWallJump = 1.5f;
	[Tooltip("Offset from the pivot when detecting collision")]
	public Vector3 collisionOffset = new Vector3(0, 0.5f, 0);

	//[Header("FX")]
	[Tooltip("Visual/Sound FX Instantiated on jump")]
	public GameObject jumpFX = null;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] Vector3 FXOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] float timeBeforeDestroyFX = 3f;

	//[Header("Animation")]
	[Tooltip("Reference to the Animator Component")]
	public Animator animator;
	[Tooltip("Name of the Trigger parameter called when wall jumping")]
	public string wallJumpTriggerName = "wallJump";

	Rigidbody rigid;

	private void Update ()
	{
		if (GroundCheck(transform.forward, collisionCheckDistance))
		{
			WallSlide();
			if (GroundCheck(Vector3.down, minimalHeightAllowedToWallJump) == false)
			{
				WallJumpCheck();
			}
		}
	}

	private void OnDrawGizmosSelected ()
	{
		Debug.DrawLine(transform.position + collisionOffset, transform.position + collisionOffset + transform.forward * (collisionCheckDistance), Color.red);
	}

	bool GroundCheck (Vector3 dir, float collisionCheckDistance)
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position + collisionOffset, dir, out hit, collisionCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.Ignore))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void WallSlide()
	{
		Vector3 gravityDampen = Physics.gravity - Physics.gravity / gravityDampenWhileOnWall;
		if(rigid.velocity.y < 0)
		{
			rigid.AddForce(gravityDampen * Time.deltaTime * -50f, ForceMode.Force);
		}
	}

	void WallJumpCheck ()
	{
		if (Input.GetButtonDown(wallJumpInputName))
		{
			if (rigid != null)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position + collisionOffset, transform.forward, out hit, collisionCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.Ignore))
				{
					if(jumpFX != null)
					{
						GameObject fx = Instantiate(jumpFX, hit.point, jumpFX.transform.rotation);
						fx.transform.up = hit.normal;

						fx.GetComponent<ParticleSystem>().Play();

						if(timeBeforeDestroyFX > 0)
						{
							Destroy(fx, timeBeforeDestroyFX);
						}
					}
				}

				WallJumpTrigger();
			}
			else
			{
				Debug.LogWarning("No Rigidbody on this GameObject !", gameObject);
			}
		}
	}

	void WallJumpTrigger ()
	{
		if (animator != null)
		{
			animator.SetTrigger(wallJumpTriggerName);
		}

		if(resetJumpCountOnWallJump && jumper != null)
		{
			jumper.currentJumpAmount = jumper.jumpAmount;
		}

		rigid.velocity = Vector3.zero;
		Vector3 force = transform.right * wallJumpForce.x + transform.up * wallJumpForce.y + transform.forward * wallJumpForce.z;
		rigid.AddForce(force);

		if (invertLookDirectionOnWallJump)
		{
			transform.forward *= -1f;
		}

		if (preventMovingAfterWallJump)
		{
			StartCoroutine(PreventMovement());
		}
	}

	public IEnumerator PreventMovement ()
	{
		mover.canMove = false;

		yield return new WaitForSeconds(movePreventionDuration);

		mover.canMove = true;
	}

	private void Start ()
	{
		InitialRefCheck();
	}

	void InitialRefCheck ()
	{
		if (rigid == null)
		{
			rigid = GetComponent<Rigidbody>();

			if (rigid == null)
			{
				Debug.LogWarning("No Rigidbody Component found on this GameObject !", gameObject);
			}
		}

		if (resetJumpCountOnWallJump && jumper == null)
		{
			jumper = GetComponent<Jumper>();

			if (jumper == null)
			{
				Debug.LogWarning("No Jumper Component found on this GameObject !", gameObject);
			}
		}

		if (preventMovingAfterWallJump && mover == null)
		{
			mover = GetComponent<Mover>();
			if (mover == null)
			{
				Debug.LogWarning("No Mover Component found on this GameObject !", gameObject);
			}
		}

		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}
}
