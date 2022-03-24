using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDestroy : MonoBehaviour
{
	[Header("Explosion")]
	[Tooltip("Damage dealt to GameObjects in range")]
	public int damage;
	[Tooltip("Range of the explosion")]
	public float range;
	[Tooltip("Directional force applied to GameObjects in range")]
	public float bumpForce;
	[Tooltip("Upwards modifier applied to the directional force")]
	public float upwardsModifier;
	[Tooltip("Layers affected by the explosion")]
	public LayerMask explosionLayerMask;

	[Header("FX")]
	[SerializeField] GameObject fxToSpawn = null;
	[SerializeField] float fxLifeTime = 3f;

	private void OnDestroy ()
	{
		Collider[] targets = Physics.OverlapSphere(transform.position, range, explosionLayerMask);
		if(targets.Length > 0)
		{
			if(damage > 0)
			{
				for(int i = 0; i < targets.Length; i++)
				{
					Life life = targets[i].GetComponent<Life>();
					if(life != null)
					{
						life.ModifLife(damage * -1);
					}
				}
			}
			foreach(Collider hit in targets)
			{
				Rigidbody rigid = hit.GetComponent<Rigidbody>();
				if(rigid != null)
				{
					rigid.AddExplosionForce(bumpForce, transform.position, range, upwardsModifier);
				}
			}
			
		}

		if(fxToSpawn != null)
		{
			GameObject fx = Instantiate(fxToSpawn, transform.position, transform.rotation);
			if(fxLifeTime > 0 )
			{
				Destroy(fx, fxLifeTime);
			}
		}

	}
}
