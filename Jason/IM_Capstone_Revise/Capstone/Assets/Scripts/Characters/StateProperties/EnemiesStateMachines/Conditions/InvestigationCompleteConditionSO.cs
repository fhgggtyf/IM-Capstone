using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that returns true when the current investigation cycle is complete.
/// The InvestigateIdleAction can set the investigationComplete flag on the
/// NonPlayerCharacter once the enemy has reached the target location and waited
/// the appropriate amount of time. When this condition evaluates to true the
/// state machine should transition the enemy back to its patrol state.
/// </summary>
[CreateAssetMenu(fileName = "InvestigationCompleteCondition", menuName = "State Machines/Conditions/Enemies/Investigation Complete")]
public class InvestigationCompleteConditionSO : StateConditionSO<InvestigationCompleteCondition>
{
}

public class InvestigationCompleteCondition : Condition
{
    private NonPlayerCharacter _npc;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
    }

    protected override bool Statement()
    {
        if (_npc.investigationComplete)
        {
            // Reset the flag so that the next investigation can proceed
            _npc.investigationComplete = false;
            _npc.hasHeardPlayer = false;
            // Clear last heard position to avoid stale data
            _npc.lastHeardPosition = Vector2.zero;
            return true;
        }
        return false;
    }
}