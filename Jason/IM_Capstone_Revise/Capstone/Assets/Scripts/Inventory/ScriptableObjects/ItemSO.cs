using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemSO : SerializableScriptableObject
{
    [Tooltip("The name of the item")]
    [SerializeField] private LocalizedString _name = default;

    [Tooltip("An icon image for the item")]
    [SerializeField]
    private Sprite _icon = default;

    [Tooltip("A description of the item")]
    [SerializeField]
    private LocalizedString _description = default;

    [SerializeField]
    private StatModification[] _effects;

    [Tooltip("The type of item")]
    [SerializeField]
    private ItemTypeSO _itemType = default;

    [SerializeField]
    private ItemRarity _rarity;

    public LocalizedString Name => _name;
    public Sprite Icon => _icon;
    public LocalizedString Description => _description;
    public StatModification[] Effects => _effects;
    public ItemTypeSO ItemType => _itemType;
    public ItemRarity Rarity => _rarity;
    //public virtual List<ItemStack> IngredientsList { get; }

    public virtual bool IsLocalized { get; }
    public virtual LocalizedSprite LocalizePreviewImage { get; }

}