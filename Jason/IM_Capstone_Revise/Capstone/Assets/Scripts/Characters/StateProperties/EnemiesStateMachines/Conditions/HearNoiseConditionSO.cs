using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that evaluates whether the enemy has heard the player via sound. It
/// calculates the distance between the enemy and the player and compares that
/// distance against the sum of the player's noise radius and the enemy's own
/// sound detection threshold. If the circles defined by these radii overlap,
/// the condition becomes true and stores the player's current position on the
/// NonPlayerCharacter for investigation.
/// </summary>
[CreateAssetMenu(fileName = "HearNoiseCondition", menuName = "State Machines/Conditions/Enemies/Hear Noise")]
public class HearNoiseConditionSO : StateConditionSO<HearNoiseCondition>
{
}

public class HearNoiseCondition : Condition
{
    private NonPlayerCharacter _npc;
    // Reference to the noise detection component on the core.  This component
    // performs the actual sensing of player noise and exposes whether a new
    // detection has occurred.
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
        // If no detector is available or the NPC reference is missing, we cannot
        // evaluate the condition.  Return false to indicate no transition.
        if (_noiseDetector == null || _npc == null)
            return false;

        // If a new noise has been detected, signal a transition into the
        // investigate state.  We do not store the last heard position on the
        // NPC here; the NoiseDetection component already tracks it and
        // consumers should read it from there.  Setting the hasHeardPlayer
        // flag on the NPC allows other parts of the state machine to know
        // that an investigation is in progress.
        if (_noiseDetector.NewDetection)
        {
            _npc.hasHeardPlayer = true;
            return true;
        }

        // If noise is currently detected but it is not a new detection,
        // do nothing.  The movement logic will read the latest position
        // directly from the NoiseDetection component.
        return false;
    }

    public override void OnStateExit()
    {
        // Do nothing on exit.  Detection flags are cleared when the
        // investigation completes via InvestigationCompleteCondition.
    }
}