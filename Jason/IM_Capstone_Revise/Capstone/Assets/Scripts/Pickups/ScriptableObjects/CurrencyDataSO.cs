using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCurrencyData", menuName = "Data/Weapon Data/Currency Data", order = 0)]
public class CurrencyDataSO : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Amount{ get; private set; }

}
