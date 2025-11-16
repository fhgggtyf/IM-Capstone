using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// This class contains all the variables that will be serialized and saved to a file.<br/>
/// Can be considered as a save file structure or format.
/// </summary>
[Serializable]
public class Save
{
	// This is test data, written according to TestScript.cs class
	// This will change according to whatever data that needs to be stored

	// The variables need to be public, else we would have to write trivial getter/setter functions.
	public string _locationId;
	public List<SerializedItemStack> _itemStacks = new List<SerializedItemStack>();
	public int _powerFragmentCnt = 0;

	//public List<string> _finishedQuestlineItemsGUIds = new List<string>();

	public float _masterVolume = default;
	public float _musicVolume = default;
	public float _sfxVolume = default;
	public int _resolutionsIndex = default;
	public int _antiAliasingIndex = default;
	public float _shadowDistance = default;
	public bool _isFullscreen = default;
    public Locale _currentLocale = default;

	public int _potentialHealthLv;
	public int _potentialArmorLv;
	public int _potentialMagicResistLv;
	public int _potentialAttackLv;
	public int _potentialAbilityPowerLv;
	public int _potentialAttackSpeedLv;
	public int _potentialManaLv;
	public int _potentialTenacityLv;
	public int _potentialStaminaLv;
	public int _potentialLuckLv;
	
	public int _baseHealthLv;
	public int _baseArmorLv;
	public int _baseMagicResistLv;
	public int _baseAttackLv;
	public int _baseAbilityPowerLv;
	public int _baseAttackSpeedLv;
	public int _baseManaLv;
	public int _baseTenacityLv;
	public int _baseStaminaLv;
	public int _baseLuckLv;

	public void SaveSettings(SettingsSO settings)
	{
		_masterVolume = settings.MasterVolume;
		_musicVolume = settings.MusicVolume;
		_sfxVolume = settings.SfxVolume;
		_resolutionsIndex = settings.ResolutionsIndex;
		_antiAliasingIndex = settings.AntiAliasingIndex;
		_shadowDistance = settings.ShadowDistance;
		_isFullscreen = settings.IsFullscreen;
        _currentLocale = settings.CurrentLocale;
    }

	public void SaveUpgrades(UpgradeConfigSO upgrades)
    {
		_potentialHealthLv = upgrades.PotentialHealthLv;
		_potentialArmorLv = upgrades.PotentialArmorLv;
		_potentialMagicResistLv = upgrades.PotentialMagicResistLv;
		_potentialAttackLv = upgrades.PotentialAttackLv;
		_potentialAbilityPowerLv = upgrades.PotentialAbilityPowerLv;
		_potentialAttackSpeedLv = upgrades.PotentialAttackSpeedLv;
		_potentialManaLv = upgrades.PotentialManaLv;
		_potentialTenacityLv = upgrades.PotentialTenacityLv;
		_potentialStaminaLv = upgrades.PotentialStaminaLv;
		_potentialLuckLv = upgrades.PotentialLuckLv;

		_baseHealthLv = upgrades.BaseHealthLv;
		_baseArmorLv = upgrades.BaseArmorLv;
		_baseMagicResistLv = upgrades.BaseMagicResistLv;
		_baseAttackLv = upgrades.BaseAttackLv;
		_baseAbilityPowerLv = upgrades.BaseAbilityPowerLv;
		_baseAttackSpeedLv = upgrades.BaseAttackSpeedLv;
		_baseManaLv = upgrades.BaseManaLv;
		_baseTenacityLv = upgrades.BaseTenacityLv;
		_baseStaminaLv = upgrades.BaseStaminaLv;
		_baseLuckLv = upgrades.BaseLuckLv;
	}

	public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}

	public void LoadFromJson(string json)
	{
		JsonUtility.FromJsonOverwrite(json, this);
	}
}
