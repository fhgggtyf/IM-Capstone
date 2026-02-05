using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that returns true when the player's hiding flag is set. This flag is
/// maintained by hide interactions and hide state actions.
/// </summary>
[CreateAssetMenu(fileName = "IsHidingCondition", menuName = "State Machines/Conditions/Is Hiding")]
public class IsHidingConditionSO : StateConditionSO<IsHidingCondition>
{
}

public class IsHidingCondition : Condition
{
    private Player _player;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    protected override bool Statement()
    {
        return _player.isHiding;
    }
}