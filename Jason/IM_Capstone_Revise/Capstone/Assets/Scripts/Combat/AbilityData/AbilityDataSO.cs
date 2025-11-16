using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "AbilityData")]
public class AbilityDataSO : ScriptableObject
{
    public DamageType type;
    public Vector2 knockBackDirection;
    public float knockBackVelocity;
    public float stunDuration;

    public bool IsEnemyAbility;

    #region Is Enemy

    public bool canBeInterupted;

    #endregion

    public AbilityDataSO()
    {
        type = DamageType.True;
    }
}
