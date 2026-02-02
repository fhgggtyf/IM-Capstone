using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Enum representing the different movement and noise states the player can occupy. The
/// MovementAndNoiseAction will interpret these values to set velocity, update noise
/// values on the StatsManager and toggle flags on the Player accordingly.
/// </summary>
public enum MoveType
{
    Idle,
    Walk,
}

/// <summary>
/// A state action that drives movement and noise output on the player. Depending on the
/// configured <see cref="MoveType"/>, this action will read the player's input vector,
/// set the Rigidbody velocity via the Movement core component and update the noise
/// level on the StatsManager. Flags on the player (isRunning, isCrouching) are
/// maintained so that conditions can cleanly reference the player's current intent.
/// </summary>
[CreateAssetMenu(fileName = "MovementAndNoiseAction", menuName = "State Machines/Actions/Movement And Noise")]
public class MoveAndNoiseActionSO : StateActionSO<MoveAndNoiseAction>
{
    public MoveType moveType;
}

public class MoveAndNoiseAction : StateAction
{
    private Player _player;
    private Movement _movement;
    private PlayerStatsManager _statsManager;
    private MoveAndNoiseActionSO _originSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        // Obtain the Movement core component from the player's core system. This allows us
        // to set the Rigidbody2D velocity without directly referencing the Rigidbody.
        _movement = _player.Core.GetCoreComponent<Movement>();
        // The stats manager resides on the same GameObject as the state machine. It
        // exposes speed and noise values derived from a StatsConfigSO.
        _statsManager = stateMachine.GetComponent<PlayerStatsManager>();
        _originSO = (MoveAndNoiseActionSO)OriginSO;
    }

    public override void OnStateEnter()
    {
        // Upon entering a new movement state, update the player's flags and set the
        // appropriate base noise level on the stats manager. Noise is kept constant
        // during the state's lifetime except when idle and holding breath/hiding.
        switch (_originSO.moveType)
        {
            case MoveType.Walk:
                _statsManager.SetCurrentNoise(_statsManager.GetWalkingNoise());
                break;
            case MoveType.Idle:
                // Idle noise will be set in OnUpdate depending on whether the player is
                // holding their breath or not. Initialise to zero here.
                _statsManager.SetCurrentNoise(0);
                break;
        }
    }

    public override void OnStateExit()
    {
    }

    public override void OnUpdate()
    {
        Debug.Log(_player.InputVector);
        // Each frame, set the player's velocity according to the movement state and
        // update the noise level if necessary.
        switch (_originSO.moveType)
        {
            case MoveType.Walk:
                {
                    float speed = _statsManager.GetWalkingSpeed();
                    Vector2 input = _player.InputVector;
                    Vector2 velocity = new Vector2(input.x, input.y) * speed;
                    _movement.SetVelocity(velocity);
                    break;
                }
            case MoveType.Idle:
                {
                    // Stop the character completely when idle.
                    _movement.SetVelocityZero();
                    // If the player is holding breath or hiding, there should be no noise. If not,
                    // keep noise at zero (idle by default makes no noise in this implementation).
                    if (_player.isHiding)
                    {
                        _statsManager.SetCurrentNoise(0);
                    }
                    else
                    {
                        _statsManager.SetCurrentNoise(_statsManager.GetIdleNoise());
                    }
                    break;
                }
        }
    }
}