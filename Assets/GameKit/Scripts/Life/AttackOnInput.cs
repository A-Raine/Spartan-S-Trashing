using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnInput : MonoBehaviour
{
	[Header("Input")]
	[Tooltip("Input name used for attacking (InputManager)")]
	[SerializeField] string inputName = "Fire1";

	[Header("Detection")]

	[Tooltip("Layers affected by the attack")]
	[SerializeField] LayerMask attackLayerMask = 1;

	[Range(0f,360f)]
	[Tooltip("Attack effective angle")]
	public float attackAngle = 90f;

	[Tooltip("Range of the attack")]
	public float attackRange = 3f;

	[Tooltip("Delay before dealing damage. Useful for syncing with animation")]
	[SerializeField] float damageDelay = 0.5f;

	[Tooltip("Cooldown between each attack")]
	public float attackCooldown = 1f;

	[Header("Hit")]

	[Tooltip("Damage applied to hit GameObjects with a Life Component")]
	[SerializeField] int attackDamage = 5;

	[Tooltip("Force applied to hit Gameobjects")]
	[SerializeField] float attackKnockback = 10f;

	[Tooltip("Vertical force applied to hit Gameobjects")]
	[SerializeField] float attackUpwardsKnockback = 5f;

	[Tooltip("FX Instantiated on hit GameObjects")]
	[SerializeField] GameObject hitFX = null;


	[Header("Animation")]
	[Tooltip("Reference to the Animator Component")]
	public Animator animator = null;

	[Tooltip("Name of the Trigger parameter called during the attack")]
	[SerializeField] string attackTriggerParameterName = "Attack";

	float effectiveRange;
	[HideInInspector] public float timer = 0f;

	// Start is called before the first frame update
	void Start()
    {
		effectiveRange = Remap(attackAngle, 0, 360f, 1, -1f);
		//Debug.Log("Effective range dot product " + effectiveRange);

		if (animator == null)
		{
			//animator = GetComponent<Animator>();
			Debug.LogWarning("No Animator found on this GameObject ! Please add one", gameObject);

		}
		
    }

	void Attack()
	{
		timer = attackCooldown;

		if(animator != null)
		{
			animator.SetTrigger(attackTriggerParameterName);
		}

		StartCoroutine(DamageDeal());
	}

	public IEnumerator DamageDeal()
	{
		yield return new WaitForSeconds(damageDelay);

		Collider[] angleEntities = Physics.OverlapSphere(transform.position, attackRange, attackLayerMask);
		foreach (Collider entity in angleEntities)
		{
			Vector3 toTarget = entity.transform.position - transform.position;
			Vector3 knockbackDir = toTarget.normalized * attackKnockback;
			knockbackDir.y = attackUpwardsKnockback;

			float dot = Vector3.Dot(transform.transform.forward, toTarget.normalized);

			// If entity is within range and in the right angle
			if (dot >= effectiveRange)
			{
				Life entityLife = entity.GetComponent<Life>();
				Rigidbody entityRigid = entity.gameObject.GetComponent<Rigidbody>();

				if (entityLife)
				{
					entityLife.ModifLife(attackDamage * -1);
				}
				else
				{
					//Debug.Log("Life component found no hit entity !");
				}

				if (entityRigid != null)
				{
					entityRigid.AddForce(knockbackDir, ForceMode.Impulse);
				}

				if(hitFX != null && (entityLife != null || entityRigid != null))
				{
					GameObject fx = Instantiate(hitFX, entity.transform.position, entity.transform.rotation);
					Destroy(fx, 3f);
				}
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
        if(timer > 0f)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			if(Input.GetButtonDown(inputName))
			{
				Attack();
			}
		}
    }

	public float Remap (float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
