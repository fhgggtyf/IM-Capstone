using System;
using UnityEngine;

public class WeaponInventory : CoreComponent
{
    [field: SerializeField] public WeaponInventorySO WeaponData { get; private set; }

    [SerializeField] private WeaponEventChannelSO _weaponEventChannel;

    public bool TrySetWeapon(WeaponDataSO newData, int index, out WeaponDataSO oldData)
    {
        if (index >= WeaponData.WeaponData.Length)
        {
            oldData = null;
            return false;
        }

        oldData = WeaponData.WeaponData[index];
        WeaponData.WeaponData[index] = newData;

        _weaponEventChannel.RaiseEvent(index, newData);

        return true;
    }

    public bool TryGetEmptyIndex(out int index)
    {
        for (var i = 0; i < WeaponData.WeaponData.Length; i++)
        {
            if (WeaponData.WeaponData[i] is not null)
                continue;

            index = i;
            return true;
        }

        index = -1;
        return false;
    }

    public WeaponSwapChoice[] GetWeaponSwapChoices()
    {
        var choices = new WeaponSwapChoice[WeaponData.WeaponData.Length];

        for (var i = 0; i < WeaponData.WeaponData.Length; i++)
        {
            var data = WeaponData.WeaponData[i];

            choices[i] = new WeaponSwapChoice(data, i);
        }

        return choices;
    }

    private void OnDisable()
    {
        WeaponData.ClearInventory();
    }
}
