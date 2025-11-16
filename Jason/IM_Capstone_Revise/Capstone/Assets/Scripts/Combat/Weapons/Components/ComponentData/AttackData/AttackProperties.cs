using System;
using UnityEngine;


[Serializable]
public class AttackProperties : AttackData
{
    [field: SerializeField] public float Amount { get; private set; }
    [field: SerializeField] public int ArmorIgnore { get; private set; }
    [field: SerializeField] public int MRIgnore { get; private set; }
    [field: SerializeField] public AbilityDataSO AbilityData { get; private set; }
}
