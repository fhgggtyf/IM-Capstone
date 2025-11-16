using System;
using UnityEngine;


[Serializable]
public class AttackActionHitBox : AttackData
{
    public bool Debug;
    [field: SerializeField] public Rect HitBox { get; private set; }
}
