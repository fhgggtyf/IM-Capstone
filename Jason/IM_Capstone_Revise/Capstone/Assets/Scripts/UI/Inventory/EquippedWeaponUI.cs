using System;
using UnityEngine;
using UnityEngine.UI;


public class EquippedWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;

    [SerializeField] private CombatInputs input;

    [SerializeField] private WeaponInventorySO _playerWeaponInventory;

    [SerializeField] private WeaponEventChannelSO _weaponEventChannel;

    //private WeaponInventory weaponInventory;
    private WeaponDataSO weaponData;

    private void SetWeaponIcon()
    {
        weaponIcon.sprite = weaponData ? weaponData.Icon : null;
        weaponIcon.color = weaponData ? Color.white : Color.clear;
    }

    private void HandleWeaponDataChanged(int inputIndex, WeaponDataSO data)
    {
        if (inputIndex != (int)input)
            return;

        weaponData = data;
        //_playerWeaponInventory.WeaponData[inputIndex] = weaponData;
        SetWeaponIcon();
    }

    private void Start()
    {
        _playerWeaponInventory.TryGetWeapon((int)input, out weaponData);
        SetWeaponIcon();
    }

    private void OnEnable()
    {
        _weaponEventChannel.OnEventRaised += HandleWeaponDataChanged;
    }

    private void OnDisable()
    {
        _weaponEventChannel.OnEventRaised -= HandleWeaponDataChanged;
    }
}
