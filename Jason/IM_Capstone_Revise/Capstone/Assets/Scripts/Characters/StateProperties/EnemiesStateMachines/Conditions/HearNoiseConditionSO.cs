
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using UnityEngine;

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

        // On a new detection, copy the position of the noise into the NPC and
        // return true to signal a transition into the investigate state.  Also
        // set the hasHeardPlayer flag on the NPC so other conditions know that
        // the investigation is in progress.
        if (_noiseDetector.NewDetection)
        {
            _npc.lastHeardPosition = _noiseDetector.LastHeardPosition;
            _npc.hasHeardPlayer = true;
            return true;
        }

        // If the detector has already registered a noise (Detected remains true)
        // but this is not a new detection, update the NPC's last heard
        // position so movement can continue tracking the player.  Do not
        // trigger a new transition.
        if (_noiseDetector.Detected)
        {
            _npc.lastHeardPosition = _noiseDetector.LastHeardPosition;
        }

        return false;
    }

    public override void OnStateExit()
    {
        // Do nothing on exit.  Detection flags are cleared when the
        // investigation completes via InvestigationCompleteCondition.
    }
}