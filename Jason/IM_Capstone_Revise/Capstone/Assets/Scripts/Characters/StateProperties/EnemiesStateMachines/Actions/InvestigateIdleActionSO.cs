using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Idle action used during the investigation state. When the enemy enters the
/// investigation state it will first wait for a specified duration (to
/// represent a "confusion" animation) before starting to move towards the
/// last heard location. After reaching that location the same action can be
/// reused with a longer duration (e.g. 2 seconds) to pause before returning
/// to patrol. Use the same action class with different durations by creating
/// separate scriptable objects.
/// </summary>
[CreateAssetMenu(fileName = "InvestigateIdleAction", menuName = "State Machines/Actions/Enemies/Investigate Idle")]
public class InvestigateIdleActionSO : StateActionSO<InvestigateIdleAction>
{
    /// <summary>
    /// The amount of time in seconds that the enemy should remain idle. For the
    /// initial confusion this might be around 1 second. For the pause after
    /// reaching the investigated location this might be around 2 seconds.
    /// </summary>
    public float idleDuration = 1f;
}

public class InvestigateIdleAction : StateAction
{
    private NonPlayerCharacter _npc;
    private InvestigateIdleActionSO _origin;
    private float _timer;
    private float _duration;
    private int _idleCount = 0;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _origin = (InvestigateIdleActionSO)OriginSO;
    }

    public override void OnStateEnter()
    {
        _timer = 0f;
        _duration = _origin.idleDuration;
        // Ensure we begin in idle state
        _npc.nonIdle = false;
        _idleCount++;
    }

    public override void OnUpdate()
    {

        if (_npc.nonIdle)
        {
            return;
        }

        // While idling, freeze movement vector
        _npc.movementVector = Vector2.zero;
        Debug.Log("Idling with movement vector set to zero");
        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            // Signal that the NPC can begin moving or, if this idle represents the
            // final pause, mark the investigation as complete. When
            // markInvestigationCompleteOnFinish is false (initial confusion) we
            // simply set nonIdle to true to begin movement. Otherwise we set
            // investigationComplete so the investigate-to-patrol condition can
            // trigger and leave nonIdle false so that no further movement
            // happens in this state.
            _timer = 0f;
            if (_idleCount == 1)
            {
                _idleCount++;
            }
            else
            {
                _idleCount = 0;
                _npc.investigationComplete = true;
                _npc.nonIdle = false;
            }

            _npc.nonIdle = true;
            
        }
    }

    public override void OnStateExit()
    {
        // Reset nonIdle when leaving idle so that subsequent states can use it
        _idleCount = 0;
        _npc.nonIdle = false;
        // We deliberately do not reset the hasHeardPlayer flag here.  Keeping this flag
        // set allows the HearNoiseCondition to continue updating the lastHeardPosition
        // while the enemy is already in the investigate state without retriggering
        // the investigate transition.  The flag is cleared when the investigation
        // completes via the InvestigationCompleteCondition.
    }
}