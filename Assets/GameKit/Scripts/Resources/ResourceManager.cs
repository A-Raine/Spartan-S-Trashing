using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	[System.Serializable] public class Resource
	{
		[Header("ID")]
		public string resourceName = "Resource Name";
		public Sprite resourceIcon = null;

		[Header("Values")]
		public int maxResourceAmount = 100;
		public int currentResourceAmount = 1;

		[Header("Display")]
		public Text resourceAmountText = null;
		public Image resourceAmountImage = null;
	}

	[SerializeField] public Resource[] resources = new Resource[1];

	public void DisplayResources (int index)
	{

		if (resources[index].resourceAmountText != null)
		{
			resources[index].resourceAmountText.text = resources[index].currentResourceAmount.ToString();
		}

		if (resources[index].resourceAmountImage != null)
		{
			resources[index].resourceAmountImage.sprite = resources[index].resourceIcon;
		}
	}

	public bool ResourceCheck(int index, int amountRequired)
	{
		if(amountRequired > resources[index].maxResourceAmount)
		{
			Debug.LogWarning("Amount required superior to maximum resource amount ! Condition will never be met", gameObject);
			return false;
		}
		else
		{
			if(resources[index].currentResourceAmount >= amountRequired)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public bool ChangeResourceAmount (int index, int amount)
	{
		//If Array Index not Out of Range
		if(index < resources.Length)
		{
			//Are we adding or substracting resources ?
			if(amount < 0)
			{
				//Do we have enough resources to substract ?
				if(resources[index].currentResourceAmount + amount < 0)
				{
					Debug.LogWarning("Trying to remove more resources than you currently have.", gameObject);
					return false;
				}
				else
				{
					resources[index].currentResourceAmount += amount;

					DisplayResources(index);
					return true;
				}
			}
			else
			{
				resources[index].currentResourceAmount += amount;
				DisplayResources(index);
				Mathf.Clamp(resources[index].currentResourceAmount, 0, resources[index].maxResourceAmount);
				return true;
			}
		}
		else
		{
			Debug.LogWarning("Wrong Index ! Remember that indexes start at 0, not 1", gameObject);
			return false;
		}
	}
	// Start is called before the first frame update
	void Start ()
	{
		for (int i = 0; i < resources.Length; i++)
		{
			DisplayResources(i);
		}
	}
}
