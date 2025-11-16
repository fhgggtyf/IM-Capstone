using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIStatTab : MonoBehaviour
{
	public UnityAction<StatTabSO> TabClicked;

	[SerializeField] private TMP_Text _tabText = default;
	[SerializeField] private Button _actionButton = default;

	[ReadOnly] public StatTabSO _currentTabType = default;

	public void SetTab(StatTabSO tabType, bool isSelected)
	{
        _currentTabType = tabType;
		_tabText.text = tabType.TabName;   
		_tabText.color = Color.black;

        UpdateState(isSelected);
	}

	public void UpdateState(bool isSelected)
	{
		_actionButton.interactable = !isSelected;
	}

	public void ClickButton()
	{
        TabClicked.Invoke(_currentTabType);
	}
}
