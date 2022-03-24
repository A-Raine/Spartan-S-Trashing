using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Spawner : MonoBehaviour
{
	[Header("Spawning options")]
	public bool shareOrientation = false;

	[System.Serializable]
	public class PrefabToSpawn
	{
		public GameObject prefab = null;
		public int weight = 1;

		public PrefabToSpawn (GameObject _prefab, int _weight)
		{
			prefab = _prefab;
			weight = _weight;
		}
	}

	public List<PrefabToSpawn> prefabToSpawn = new List<PrefabToSpawn>();
	//public List<GameObject> prefabToSpawn = new List<GameObject>(1);
	public Vector3 randomMinOffset = Vector3.zero;
	public Vector3 randomMaxOffset = Vector3.zero;

	[Header("Cooldown")]
	public bool useCooldown = false;
	public float spawnCooldown = 1f;

	[Header("Input")]
	public bool useInput = false;
	public string spawnInputName = "Input Name (InputManager)";

	[Header("Nested spawning")]
	public bool spawnInsideParent = true;
	public Transform parent;

	[Header("Resources")]
	public bool requireResources = false;
	public ResourceManager resourceManager;
	public int resourceIndex = 0;
	public int resourceCostOnUse = 1;
	public bool destroyOnNoResourceLeft = false;

	float timer = 0f;

	void Awake ()
	{
		
	}
	void Start ()
	{
		InitialDebugMessages();
	}

	void Update ()
	{
		SpawnManagement();
	}

	bool TimerManagement ()
	{
		if (timer < spawnCooldown)
		{
			timer += Time.deltaTime;
			return false;
		}
		else
		{
			timer = spawnCooldown;
			return true;
		}
	}

	GameObject WeightedSpawn ()
	{

		int totalSize = 0;
		int currentWeight = 0;

		for (int i = prefabToSpawn.Count - 1; i >= 0; i--)
		{
			if (prefabToSpawn[i].prefab != null)
			{
				totalSize += prefabToSpawn[i].weight;
			}
			else
			{
				prefabToSpawn.Remove(prefabToSpawn[i]);
				Debug.LogWarning("Empty entry in prefabs to spawn ! Removing it", gameObject);
			}
		}

		if(totalSize != 0)
		{
			int randomWeight = Random.Range(0, totalSize + 1);

			foreach (PrefabToSpawn p in prefabToSpawn)
			{
				int newWeight = currentWeight + p.weight;
				if (randomWeight <= newWeight)
				{
					//Debug.Log("Weighted spawn succeeded ! Spawning " + p.prefab.name);
					return p.prefab;
				}
				currentWeight = newWeight;
			}

			//Debug.Log("Weighted spawn failed");
			return prefabToSpawn[0].prefab;
		}
		else
		{
			return null;
		}
	}

	public void SpawnObject ()
	{
		if (prefabToSpawn.Count > 0)
		{
			GameObject gameObjectSpawned;
			GameObject prefabToSpawn = WeightedSpawn();

			if(prefabToSpawn == null)
			{
				return;
			}

			//Nested Spawning Check
			if (spawnInsideParent)
			{
				if (parent == null)
				{
					//Debug.LogWarning("Parent not set for nested spawning ! Setting parent as this GameObject");
					parent = transform;
				}

				if (shareOrientation)
				{
					gameObjectSpawned = Instantiate(prefabToSpawn, parent.transform.position, parent.transform.rotation);
				}
				else
				{
					gameObjectSpawned = Instantiate(prefabToSpawn, parent.transform.position, prefabToSpawn.transform.rotation);
				}

				//Setting parent and local position offset
				gameObjectSpawned.transform.SetParent(parent);
				gameObjectSpawned.transform.localPosition = GetRandomOffset();
			}
			else
			{
				if (shareOrientation)
				{
					gameObjectSpawned = Instantiate(prefabToSpawn, transform.position + GetRandomOffset(), transform.rotation);
				}
				else
				{
					gameObjectSpawned = Instantiate(prefabToSpawn, transform.position + GetRandomOffset(), prefabToSpawn.transform.rotation);
				}
			}
		}
		else
		{
			Debug.LogError("No prefab to spawn set !", gameObject);
		}

	}

	void SpawnManagement ()
	{
		if (useInput && useCooldown)
		{
			if (TimerManagement())
			{
				if (Input.GetButton(spawnInputName))
				{
					if (requireResources)
					{
						if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
						{
							SpawnObject();
							timer = 0f;
						}
						else if(destroyOnNoResourceLeft)
						{
							Destroy(gameObject);
						}
					}
					else
					{
						SpawnObject();
						timer = 0f;
					}
				}
			}
		}
		else
		{
			if (useInput)
			{
				if (Input.GetButtonDown(spawnInputName))
				{
					if (requireResources)
					{
						if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
						{
							SpawnObject();
						}
						else if (destroyOnNoResourceLeft)
						{
							Destroy(gameObject);
						}
					}
					else
					{
						SpawnObject();
					}
				}
			}
			else if (useCooldown)
			{
				if (TimerManagement())
				{
					if (requireResources)
					{
						if (resourceManager.ChangeResourceAmount(resourceIndex, resourceCostOnUse * -1))
						{
							SpawnObject();
							timer = 0f;
						}
						else if (destroyOnNoResourceLeft)
						{
							Destroy(gameObject);
						}
					}
					else
					{
						SpawnObject();
						timer = 0f;
					}
				}
			}
			else
			{
				Debug.Log("Add either Input or Timer based spawning !", gameObject);
			}
		}
	}

	Vector3 GetRandomOffset ()
	{
		Vector3 offset = new Vector3(Random.Range(randomMinOffset.x, randomMaxOffset.x), Random.Range(randomMinOffset.y, randomMaxOffset.y), Random.Range(randomMinOffset.z, randomMaxOffset.z));
		return offset;
	}

	void InitialDebugMessages ()
	{
		if (prefabToSpawn.Count == 0 || prefabToSpawn[0] == null)
		{
			Debug.LogWarning("No Prefab to spawn set !! Please add one", gameObject);
		}

		if (requireResources)
		{
			if (resourceManager == null)
			{
				Debug.LogWarning("Resource Manager not referenced ! Trying to find one", gameObject);

				resourceManager = GetComponent<ResourceManager>();

				if (resourceManager == null)
				{
					Debug.LogWarning("Resource Manager not found on this GameObject ! Please add or reference one", gameObject);
				}
			}
		}
	}

}
