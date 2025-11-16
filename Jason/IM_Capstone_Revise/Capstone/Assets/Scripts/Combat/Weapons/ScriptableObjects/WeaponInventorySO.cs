using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInventory", menuName = "EntityConfig/WeaponInventory")]
public class WeaponInventorySO : ScriptableObject
{
    [SerializeField] private WeaponDataSO[] _weaponData;

    public WeaponDataSO[] WeaponData { get => _weaponData; set => _weaponData = value; }

    public bool TryGetWeapon(int index, out WeaponDataSO data)
    {
        if (index >= _weaponData.Length)
        {
            data = null;
            return false;
        }

        data = _weaponData[index];
        return true;
    }

    public void ClearInventory()
    {
        Array.Clear(_weaponData, 0, _weaponData.Length);
    }
}
