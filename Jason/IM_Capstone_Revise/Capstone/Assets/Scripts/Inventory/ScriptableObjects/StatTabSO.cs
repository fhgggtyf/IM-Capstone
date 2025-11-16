using System;
using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum StatTabType
{
	Stats,
	Potential
}

[CreateAssetMenu(fileName = "StatTabType", menuName = "Inventory/Stat Tab Type")]
public class StatTabSO : ScriptableObject
{
	[SerializeField] private string _tabName = default;
	[SerializeField] private StatTabType _tabType = default;

    public string TabName => _tabName;
	public StatTabType TabType => _tabType;
}
