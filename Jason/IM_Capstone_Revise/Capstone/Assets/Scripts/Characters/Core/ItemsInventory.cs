using System;
using UnityEngine;

public class ItemsInventory : CoreComponent
{
    [field: SerializeField] public InventorySO Data { get; private set; }

    [SerializeField] private ItemStackEventChannelSO _itemEventChannel;

    public bool TryAddItem(ItemSO newData, int count)
    {
        //if (index >= Data.Items.Capacity)
        //{
        //    oldData = null;
        //    return false;
        //}

        //oldData = Data.Items[index].Item;
        //Data.Add(newData, count);

        _itemEventChannel.RaiseEvent(new ItemStack(newData, count));

        return true;
    }

    public bool TryGetEmptyIndex()
    {
        //for (var i = 0; i < Data.maxCapacity; i++)
        //{
        //    if (Data.Items[i].Item != null)
        //        continue;

        //    index = i;
        //    return true;
        //}

        //index = -1;
        //return false;

        Debug.Log(Data.Items.Count);
        Debug.Log(Data.maxCapacity);
        if (Data.Items.Count < Data.maxCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //public WeaponSwapChoice[] GetWeaponSwapChoices()
    //{
    //    var choices = new WeaponSwapChoice[WeaponData.WeaponData.Length];

    //    for (var i = 0; i < WeaponData.WeaponData.Length; i++)
    //    {
    //        var data = WeaponData.WeaponData[i];

    //        choices[i] = new WeaponSwapChoice(data, i);
    //    }

    //    return choices;
    //}

    private void OnDisable()
    {
        //Data.ClearInventory();
    }
}
