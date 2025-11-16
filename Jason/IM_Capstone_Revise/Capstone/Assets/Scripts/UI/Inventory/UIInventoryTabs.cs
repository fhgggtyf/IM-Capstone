using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTabs : MonoBehaviour
{
	[SerializeField] private List<UIInventoryTab> _instantiatedGameObjects;

	public event UnityAction<InventoryTabSO> TabChanged;

	private bool _canDisableLayout = false;

	void OnEnable()
    {
		if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && _canDisableLayout)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
			_canDisableLayout = false;
		}

		foreach (UIInventoryTab i in _instantiatedGameObjects)
        {
			i.TabClicked += ChangeTab;
		}
	}

	public void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
	{
		if (_instantiatedGameObjects == null)
			_instantiatedGameObjects = new List<UIInventoryTab>();

		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

		int maxCount = Mathf.Max(typesList.Count, _instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < typesList.Count)
			{
				if (i >= _instantiatedGameObjects.Count)
				{
					Debug.LogError("Maximum tabs reached");
				}
				bool isSelected = typesList[i] == selectedType;
				//fill
				_instantiatedGameObjects[i].SetTab(typesList[i], isSelected);
				_instantiatedGameObjects[i].gameObject.SetActive(true);
			}
			else if (i < _instantiatedGameObjects.Count)
			{
				//Desactive
				_instantiatedGameObjects[i].gameObject.SetActive(false);
			}
		}
		if (isActiveAndEnabled) // check if the game object is active and enabled so that we could start the coroutine. 
		{
			StartCoroutine(waitBeforeDesactiveLayout());
		}
		else // if the game object is inactive, disabling the layout will happen on onEnable 
		{
			_canDisableLayout = true;
		}
	}

	IEnumerator waitBeforeDesactiveLayout()
	{
		yield return new WaitForSeconds(1);
		//disable layout group after layout calculation
		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		{
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
			_canDisableLayout = false;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < _instantiatedGameObjects.Count; i++)
		{

			_instantiatedGameObjects[i].TabClicked -= ChangeTab;
		}
	}

	void ChangeTab(InventoryTabSO newTabType)
	{
		TabChanged.Invoke(newTabType);
	}
}
