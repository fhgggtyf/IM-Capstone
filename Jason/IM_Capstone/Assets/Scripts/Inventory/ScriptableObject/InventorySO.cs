using System.Collections.Generic;
using UnityEngine;

// Created with collaboration with:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    [Tooltip("The collection of items and their quantities.")]
    [SerializeField] private List<ItemStack> _items = new List<ItemStack>();
    [SerializeField] private List<ItemStack> _defaultItems = new List<ItemStack>();

    public List<ItemStack> Items => _items;

    [SerializeField] public int maxCapacity;

    public void Init()
    {
        if (_items == null)
        {
            _items = new List<ItemStack>(maxCapacity);
        }
        _items.Clear();
        foreach (ItemStack item in _defaultItems)
        {
            _items.Add(new ItemStack(item));
        }
    }

    public void Add(ItemSO item, int count = 1)
    {
        if (count <= 0)
            return;

        for (int i = 0; i < _items.Count; i++)
        {
            ItemStack currentItemStack = _items[i];
            if (item == currentItemStack.Item)
            {
                currentItemStack.Amount += count;

                return;
            }
        }
        Debug.Log("Cannot Find in INventory, Creating the item " + count + " times");
        _items.Add(new ItemStack(item, count));
    }

    public void Remove(ItemSO item, int count = 1)
    {
        if (count <= 0)
            return;

        for (int i = 0; i < _items.Count; i++)
        {
            ItemStack currentItemStack = _items[i];

            if (currentItemStack.Item == item)
            {
                currentItemStack.Amount -= count;

                if (currentItemStack.Amount <= 0)
                    _items.Remove(currentItemStack);

                return;
            }
        }
    }

    public bool Contains(ItemSO item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (item == _items[i].Item)
            {
                return true;
            }
        }

        return false;
    }

    public int Count(ItemSO item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            ItemStack currentItemStack = _items[i];
            if (item == currentItemStack.Item)
            {
                return currentItemStack.Amount;
            }
        }

        return 0;
    }

    //public bool[] IngredientsAvailability(List<ItemStack> ingredients)
    //{
    //	if (ingredients == null)
    //		return null;
    //	bool[] availabilityArray = new bool[ingredients.Count];

    //	for (int i = 0; i < ingredients.Count; i++)
    //	{
    //		availabilityArray[i] = _items.Exists(o => o.Item == ingredients[i].Item && o.Amount >= ingredients[i].Amount);

    //	}
    //	return availabilityArray;


    //}
    //public bool hasIngredients(List<ItemStack> ingredients)
    //{

    //	bool hasIngredients = !ingredients.Exists(j => !_items.Exists(o => o.Item == j.Item && o.Amount >= j.Amount));

    //	return hasIngredients;


    //}
}
