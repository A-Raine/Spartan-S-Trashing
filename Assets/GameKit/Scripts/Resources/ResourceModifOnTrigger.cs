using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceModifOnTrigger : MonoBehaviour
{
	[Header("Tag")]
	[SerializeField] bool useTag = false;
	[SerializeField] string tagName = "Case Sensitive";

	[Header("Resources")]
	[Tooltip("Do we use the Resource Manager Component of the colliding object or a fixed one ?")]
	[SerializeField] bool useCollidingResourceManager = false;
	[SerializeField] ResourceManager resourceManager = null;
	[SerializeField] int resourceIndex = 0;
	[SerializeField] int resourceAmount = 0;

	[Header("Other")]
	[SerializeField] GameObject spawnedFXOnTrigger = null;
	[SerializeField] float FXLifeTime = 3f;
	[SerializeField] bool destroyAfter = false;


    // Start is called before the first frame update
    void Start()
    {
        if(resourceManager == null)
		{
			//Debug.LogWarning("No Resource Manager referenced ! Please add one", gameObject);
			resourceManager = FindObjectOfType<ResourceManager>();
			if (resourceManager == null)
			{
				Debug.LogWarning("No Resource Manager referenced ! Please add one", gameObject);
			}
		}

    }

	void ModifResource(Collider other)
	{
		ResourceManager manager;

		if(useCollidingResourceManager)
		{
			manager = other.gameObject.GetComponent<ResourceManager>();

			if(manager == null && resourceManager != null)
			{
				manager = resourceManager;
			}
		}
		else
		{
			manager = resourceManager;
		}

		manager.ChangeResourceAmount(resourceIndex, resourceAmount);

		if(spawnedFXOnTrigger != null)
		{
			GameObject fX = Instantiate(spawnedFXOnTrigger, transform.position, Quaternion.identity);
			Destroy(fX, FXLifeTime);
		}

		if(destroyAfter)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter (Collider other)
	{
		if(useTag)
		{
			Rigidbody rigid = other.GetComponent<Rigidbody>();
			if (rigid == null)
			{
				rigid = other.GetComponentInParent<Rigidbody>();
			}
			string tagCheck = rigid.gameObject.tag;

			if (tagName == tagCheck)
			{
				ModifResource(other);
			}
		}
		else
		{
			ModifResource(other);
		}
	}
}
