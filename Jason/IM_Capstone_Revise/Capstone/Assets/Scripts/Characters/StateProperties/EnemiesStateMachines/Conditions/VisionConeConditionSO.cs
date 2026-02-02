using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that evaluates the enemy's vision cone to determine if the player
/// should be engaged. It queries the VisionCone core component's
/// <see cref="VisionCone.IsPlayerInSight"/> property and returns true when
/// the player is visible. When the player is seen the NonPlayerCharacter's
/// hasHeardPlayer flag is cleared so that any ongoing investigation is
/// interrupted and the enemy transitions into engage state immediately.
/// </summary>
[CreateAssetMenu(fileName = "VisionConeCondition", menuName = "State Machines/Conditions/Enemies/Vision Cone")]
public class VisionConeConditionSO : StateConditionSO<VisionConeCondition>
{
}

public class VisionConeCondition : Condition
{
    private NonPlayerCharacter _npc;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
    }

    protected override bool Statement()
    {
        return _npc.playerIsInSight;
    }
}