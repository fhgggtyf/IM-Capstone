using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemCount = default;
    [SerializeField] private Image _itemPreviewImage = default;
    [SerializeField] private Image _bgImage = default;
    [SerializeField] private Image _imgHover = default;
    [SerializeField] private Image _imgSelected = default;
    [SerializeField] private Image _bgInactiveImage = default;
    [SerializeField] private MultiInputButton _itemButton = default;
    [SerializeField] private LocalizeSpriteEvent _bgLocalizedImage = default;

    public UnityAction<ItemSO> ItemSelected;

    [HideInInspector] public ItemStack currentItem;

    bool _isSelected = false;

    public void SetItem(ItemStack itemStack, bool isSelected)
    {
        _isSelected = isSelected;
        _itemPreviewImage.gameObject.SetActive(true);
        _itemCount.gameObject.SetActive(true);
        _bgImage.gameObject.SetActive(true);
        _imgHover.gameObject.SetActive(true);
        _imgSelected.gameObject.SetActive(true);
        _itemButton.gameObject.SetActive(true);
        _bgInactiveImage.gameObject.SetActive(false);

        UnhoverItem();
        currentItem = itemStack;

        _imgSelected.gameObject.SetActive(isSelected);

        if (itemStack.Item.IsLocalized)
        {
            _bgLocalizedImage.enabled = true;
            _bgLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
        }
        else
        {
            _bgLocalizedImage.enabled = false;
            _itemPreviewImage.sprite = itemStack.Item.Icon;
        }
        Debug.Log("text here is" + itemStack.Amount.ToString());
        _itemCount.text = itemStack.Amount.ToString();

        switch (itemStack.Item.Rarity)
        {
            case ItemRarity.Common:
                _bgImage.color = Color.gray;
                break;
            case ItemRarity.Rare:
                _bgImage.color = Color.green;
                break;
            case ItemRarity.Epic:
                _bgImage.color = Color.cyan;
                break;
            case ItemRarity.Mythic:
                _bgImage.color = Color.yellow;
                break;
            case ItemRarity.Legendary:
                _bgImage.color = Color.red;
                break;

        }
    }

    public void SetInactiveItem()
    {
        UnhoverItem();
        currentItem = null;
        _itemPreviewImage.gameObject.SetActive(false);
        _itemCount.gameObject.SetActive(false);
        _bgImage.gameObject.SetActive(false);
        _imgHover.gameObject.SetActive(false);
        _imgSelected.gameObject.SetActive(false);
        _itemButton.gameObject.SetActive(false);
        _bgInactiveImage.gameObject.SetActive(true);
    }

    public void SelectFirstElement()
    {
        _isSelected = true;
        _itemButton.Select();
        SelectItem();
    }

    private void OnEnable()
    {
        if (_isSelected)
        { SelectItem(); }
    }

    public void HoverItem()
    {
        _imgHover.gameObject.SetActive(true);
    }

    public void UnhoverItem()
    {
        _imgHover.gameObject.SetActive(false);
    }

    public void SelectItem()
    {
        _isSelected = true;
        if (ItemSelected != null && currentItem != null && currentItem.Item != null)

        {
            _imgSelected.gameObject.SetActive(true);
            ItemSelected.Invoke(currentItem.Item);
        }
        else
        {
            _imgSelected.gameObject.SetActive(false);
        }
    }

    public void UnselectItem()
    {
        _isSelected = false;
        _imgSelected.gameObject.SetActive(false);
    }
}