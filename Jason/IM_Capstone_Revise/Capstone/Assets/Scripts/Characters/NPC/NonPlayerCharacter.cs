using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character
{
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    public EnemyPropertiesConfigSO entityData;
    [SerializeField]
    public VoidEventChannelSO playerIsDeadChannel;

    [NonSerialized] public bool nonIdle = false;
    [NonSerialized] public bool targetIsDead = false;

    protected virtual void OnEnable()
    {
        playerIsDeadChannel.OnEventRaised += TargetDead;
    }

    protected virtual void OnDisable()
    {
        playerIsDeadChannel.OnEventRaised -= TargetDead;
    }

    private void TargetDead() => targetIsDead = true;

    private void AttackFinished() => isAttackFinished = true;

    public virtual void OnDrawGizmos()
    {

    }

}
