using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum ItemInventoryType
{
    Weapon,
    Currency,
    Blueprint,
    Spendables
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Mythic,
    Legendary
}

public enum ItemInventoryActionType
{
    Equip,
    Spend,
    Use,
    DoNothing
}

[CreateAssetMenu(fileName = "ItemType", menuName = "Inventory/ItemType")]
public class ItemTypeSO : ScriptableObject
{
    [SerializeField] private LocalizedString _actionName = default;
    [Tooltip("The Item's background color in the UI")]
    //[SerializeField] private Color _typeColor = default;
    [SerializeField] private ItemInventoryType _type = default;
    [SerializeField] private ItemInventoryActionType _actionType = default;
    [Tooltip("The tab type under which the item will be added")]
    [SerializeField] private InventoryTabSO _tabType = default;

    public LocalizedString ActionName => _actionName;
    //public Color TypeColor => _typeColor;
    public ItemInventoryActionType ActionType => _actionType;
    public ItemInventoryType Type => _type;
    public InventoryTabSO TabType => _tabType;
}
