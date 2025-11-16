using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
    [field: SerializeField] public int NumberOfAttacks { get; private set; }
    [field: SerializeField] public List<bool> CanBeInterupted { get; private set; } 

    [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

    public void InitializeInteruptParam()
    {
        CanBeInterupted.AddRange(Enumerable.Repeat(false, NumberOfAttacks - CanBeInterupted.Count));
    }

    public T GetData<T>()
    {
        return ComponentData.OfType<T>().FirstOrDefault();
    }

    public List<Type> GetAllDependencies()
    {
        return ComponentData.Select(component => component.ComponentDependency).ToList();
    }

    public void AddData(ComponentData data)
    {
        if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null)
            return;

        ComponentData.Add(data);
    }
}