using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// This action handles the behaviour while the player is hiding inside a hideable object.
/// When the hiding state is entered the player is repositioned to the hideable's
/// transform, movement is stopped and noise is muted. It also maintains the
/// isHiding flag on the player so that conditions can detect when the hiding state
/// should end.
/// </summary>
[CreateAssetMenu(fileName = "HideAction", menuName = "State Machines/Actions/Hide")]
public class HideActionSO : StateActionSO<HideAction>
{
    public BoolEventChannelSO onHideEvent;
}

public class HideAction : StateAction
{
    private Player _player;
    private Movement _movement;
    private PlayerStatsManager _statsManager;
    private BoolEventChannelSO onHideEvent;

    HideActionSO _originSO => (HideActionSO)base.OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _movement = _player.Core.GetCoreComponent<Movement>();
        _statsManager = stateMachine.GetComponent<PlayerStatsManager>();
        onHideEvent = _originSO.onHideEvent;
    }

    public override void OnStateEnter()
    {
        // Flag the player as hiding so that other systems know the state is active.
        _player.isHiding = true;

        onHideEvent.RaiseEvent(true);
        Debug.Log("Player has entered hiding state.");

        // Mute all noise while hiding.
        _statsManager.SetCurrentNoise(0);

        _movement.ForceChangePositionZ(97);

        // Ensure the player isn't moving when entering the hide state.
        _movement.SetVelocityZero();
    }

    public override void OnUpdate()
    {
        // Continuously ensure the player remains stationary while hiding.
        _movement.SetVelocityZero();
        // Maintain zero noise output.
        _statsManager.SetCurrentNoise(0);
    }

    public override void OnStateExit()
    {
        // Clear hiding flags when exiting the hide state.
        _player.isHiding = false;
        _player.hideTarget = null;
        _movement.ForceChangePositionZ(0);
        onHideEvent.RaiseEvent(false);
        Debug.Log("Player has exited hiding state.");
    }
}