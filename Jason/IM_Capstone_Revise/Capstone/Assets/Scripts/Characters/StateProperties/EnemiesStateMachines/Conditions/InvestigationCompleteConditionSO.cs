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
    private NoiseDetection _noiseDetector;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        if (_npc != null && _npc.Core != null)
        {
            _noiseDetector = _npc.Core.GetCoreComponent<NoiseDetection>();
        }
    }

    protected override bool Statement()
    {
        if (_npc.investigationComplete)
        {
            Debug.Log("Investigation complete condition met");
            // Reset the flag so that the next investigation can proceed
            _npc.investigationComplete = false;
            // Clear NPC flags and last heard data
            _npc.hasHeardPlayer = false;
            _npc.lastHeardPosition = Vector2.zero;
            // Clear detection on the noise detector if available
            if (_noiseDetector != null)
            {
                _noiseDetector.ClearDetection();
            }
            return true;
        }
        return false;
    }
}