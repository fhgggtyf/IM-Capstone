using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "EntityConfig/Updrade Config")]
public class UpgradeConfigSO : SerializableScriptableObject
{
    [SerializeField] private int _pointsAvailable;

	[SerializeField] private int _PotentialHealthLv;
	[SerializeField] private int _PotentialArmorLv;
	[SerializeField] private int _PotentialMagicResistLv;
	[SerializeField] private int _PotentialAttackLv;
	[SerializeField] private int _PotentialAbilityPowerLv;
	[SerializeField] private int _PotentialAttackSpeedLv;
	[SerializeField] private int _PotentialManaLv;
	[SerializeField] private int _PotentialTenacityLv;
	[SerializeField] private int _PotentialStaminaLv;
	[SerializeField] private int _PotentialLuckLv;		  
	
	[SerializeField] private int _BaseHealthLv;
	[SerializeField] private int _BaseArmorLv;
	[SerializeField] private int _BaseMagicResistLv;
	[SerializeField] private int _BaseAttackLv;
	[SerializeField] private int _BaseAbilityPowerLv;
	[SerializeField] private int _BaseAttackSpeedLv;
	[SerializeField] private int _BaseManaLv;
	[SerializeField] private int _BaseTenacityLv;
	[SerializeField] private int _BaseStaminaLv;
	[SerializeField] private int _BaseLuckLv;

    public int PointsAvailable { get => _pointsAvailable; set => _pointsAvailable = value; }
    public int PotentialHealthLv { get => _PotentialHealthLv; set => _PotentialHealthLv = value; }
    public int PotentialArmorLv { get => _PotentialArmorLv; set => _PotentialArmorLv = value; }
    public int PotentialMagicResistLv { get => _PotentialMagicResistLv; set => _PotentialMagicResistLv = value; }
    public int PotentialAttackLv { get => _PotentialAttackLv; set => _PotentialAttackLv = value; }
    public int PotentialAbilityPowerLv { get => _PotentialAbilityPowerLv; set => _PotentialAbilityPowerLv = value; }
    public int PotentialAttackSpeedLv { get => _PotentialAttackSpeedLv; set => _PotentialAttackSpeedLv = value; }
    public int PotentialManaLv { get => _PotentialManaLv; set => _PotentialManaLv = value; }
    public int PotentialTenacityLv { get => _PotentialTenacityLv; set => _PotentialTenacityLv = value; }
    public int PotentialStaminaLv { get => _PotentialStaminaLv; set => _PotentialStaminaLv = value; }
    public int PotentialLuckLv { get => _PotentialLuckLv; set => _PotentialLuckLv = value; }
    public int BaseHealthLv { get => _BaseHealthLv; set => _BaseHealthLv = value; }
    public int BaseArmorLv { get => _BaseArmorLv; set => _BaseArmorLv = value; }
    public int BaseMagicResistLv { get => _BaseMagicResistLv; set => _BaseMagicResistLv = value; }
    public int BaseAttackLv { get => _BaseAttackLv; set => _BaseAttackLv = value; }
    public int BaseAbilityPowerLv { get => _BaseAbilityPowerLv; set => _BaseAbilityPowerLv = value; }
    public int BaseAttackSpeedLv { get => _BaseAttackSpeedLv; set => _BaseAttackSpeedLv = value; }
    public int BaseManaLv { get => _BaseManaLv; set => _BaseManaLv = value; }
    public int BaseTenacityLv { get => _BaseTenacityLv; set => _BaseTenacityLv = value; }
    public int BaseStaminaLv { get => _BaseStaminaLv; set => _BaseStaminaLv = value; }
    public int BaseLuckLv { get => _BaseLuckLv; set => _BaseLuckLv = value; }

    public int[] GetPotentialLevelsArray()
    {
        return new int[]
        {
        _PotentialHealthLv,
        _PotentialArmorLv,
        _PotentialMagicResistLv,
        _PotentialAttackLv,
        _PotentialAbilityPowerLv,
        _PotentialAttackSpeedLv,
        _PotentialManaLv,
        _PotentialTenacityLv,
        _PotentialStaminaLv,
        _PotentialLuckLv
        };
    }
    public int[] GetBaseLevelsArray()
    {
        return new int[]
        {
        _BaseHealthLv,
        _BaseArmorLv,
        _BaseMagicResistLv,
        _BaseAttackLv,
        _BaseAbilityPowerLv,
        _BaseAttackSpeedLv,
        _BaseManaLv,
        _BaseTenacityLv,
        _BaseStaminaLv,
        _BaseLuckLv
        };
    }
}


