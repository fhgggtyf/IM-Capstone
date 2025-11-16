using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIStatTabs : MonoBehaviour
{
	[SerializeField] private List<UIStatTab> _instantiatedGameObjects;

    public event UnityAction<StatTabSO> TabChanged;

    private bool _canDisableLayout = false;

	public void SetTabs(List<StatTabSO> typesList, StatTabSO selectedType)
	{
		if (_instantiatedGameObjects == null)
			_instantiatedGameObjects = new List<UIStatTab>();

		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

		int maxCount = Mathf.Max(typesList.Count, _instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			Debug.Log($"Setting tab {i}");
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

	public void ChangeTabSelection(StatTabSO selectedType)
	{
		for (int i = 0; i < _instantiatedGameObjects.Count; i++)
		{
			bool isSelected = _instantiatedGameObjects[i]._currentTabType == selectedType;
			//fill
			_instantiatedGameObjects[i].UpdateState(isSelected);
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < _instantiatedGameObjects.Count; i++)
		{

			_instantiatedGameObjects[i].TabClicked -= ChangeTab;
		}
	}

    void OnEnable()
    {
        if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && _canDisableLayout)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            _canDisableLayout = false;
        }

        foreach (UIStatTab i in _instantiatedGameObjects)
        {
            i.TabClicked += ChangeTab;
        }
    }

    void ChangeTab(StatTabSO newTabType)
	{
		TabChanged.Invoke(newTabType);
    }


}
