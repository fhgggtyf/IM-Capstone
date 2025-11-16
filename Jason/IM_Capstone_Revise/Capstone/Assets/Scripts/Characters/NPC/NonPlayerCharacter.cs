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

    [SerializeField] public GameObject alertSymbol, lookForPlayerSymbol, stunnedSymbol;

    [NonSerialized] public bool shouldTransit = false;
    [NonSerialized] public bool targetIsDead = false;

    protected virtual void OnEnable()
    {
        animationEventHandler.OnFinish += AttackFinished;
        playerIsDeadChannel.OnEventRaised += TargetDead;
    }

    protected virtual void OnDisable()
    {
        animationEventHandler.OnFinish -= AttackFinished;
        playerIsDeadChannel.OnEventRaised -= TargetDead;
    }

    private void TargetDead() => targetIsDead = true;

    private void AttackFinished() => isAttackFinished = true;

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer) && !targetIsDead;
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer) && !targetIsDead;
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer) && !targetIsDead;
    }

    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {

            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
        }
    }

}
