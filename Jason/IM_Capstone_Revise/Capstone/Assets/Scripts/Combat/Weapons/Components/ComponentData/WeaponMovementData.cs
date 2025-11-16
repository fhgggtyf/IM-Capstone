using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovementData : ComponentData<AttackMovement>
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(WeaponMovement);
    }
}

