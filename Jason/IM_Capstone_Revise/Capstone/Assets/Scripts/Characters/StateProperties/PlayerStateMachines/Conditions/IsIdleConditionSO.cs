using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that evaluates to true when the player is not moving beyond a threshold. This
/// is useful for transitioning back into the idle state.
/// </summary>
[CreateAssetMenu(fileName = "IsIdleCondition", menuName = "State Machines/Conditions/Is Idle")]
public class IsIdleConditionSO : StateConditionSO<IsIdleCondition>
{
    public float threshold = 0.1f;
}

public class IsIdleCondition : Condition
{
    private Player _player;
    private IsIdleConditionSO _originSO => (IsIdleConditionSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    protected override bool Statement()
    {
        return _player.InputVector.sqrMagnitude <= _originSO.threshold * _originSO.threshold;
    }
}