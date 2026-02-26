using UnityEngine;
using UnityEngine.AI;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Action that drives the enemy towards the player in the engage state. This
/// movement is non‑escapable: the enemy will pursue the player's current
/// position each frame at the configured running speed. Once the engage
/// condition is met (player visible) this action will continue indefinitely
/// until the state machine is transitioned away externally (e.g. on player
/// death or cutscene).
/// </summary>
[CreateAssetMenu(fileName = "EngageMovementAction", menuName = "State Machines/Actions/Enemies/Engage Move")]
public class EngageMovementActionSO : StateActionSO<EngageMovementAction>
{
    /// <summary>
    /// Optional multiplier applied to the base running speed when engaging the
    /// player. This allows designers to tune how relentless the enemy pursuit is.
    /// A value of 1 uses the running speed directly.
    /// </summary>
    public float speedMultiplier = 1f;
    public TransformAnchor PlayerTransformAnchor;
}

public class EngageMovementAction : StateAction
{
    private NonPlayerCharacter _npc;
    private NonPlayerStatsManager _stats;
    private Movement _movement;
    private EngageMovementActionSO _origin;
    private TransformAnchor _playerTransformAnchor;

    // Navigation controller for moving along a NavMesh. When available
    // the agent will follow paths instead of moving directly toward the target.
    private NavMeshAgentController _navController;
    // Flag indicating whether navigation is in use for this action. If false
    // movement falls back to direct velocity updates.
    private bool _useNavMesh;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _stats = stateMachine.GetComponent<NonPlayerStatsManager>();
        _movement = _npc.Core.GetCoreComponent<Movement>();
        _origin = (EngageMovementActionSO)OriginSO;
        _playerTransformAnchor = _origin.PlayerTransformAnchor;

        // Attempt to locate a NavMeshAgentController on the Core. This wrapper
        // integrates the underlying Unity NavMeshAgent with our 2D movement
        // system. If present and on a baked NavMesh, it will be used for
        // navigation instead of direct movement.
        _navController = _npc.Core.GetCoreComponent<NavMeshAgentController>();
    }

    public override void OnStateEnter()
    {
        // Ensure we are in movement mode
        _npc.nonIdle = true;

        // Initialise navigation usage on state entry. Reset flag in case
        // navigation was used previously.
        _useNavMesh = false;
        // Only use the NavMesh when the controller and agent exist and the
        // agent is positioned on a baked NavMesh surface. Otherwise we will
        // fall back to simple velocity based movement.
        if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        {
            // Align the arrival threshold with the NPC's arrive distance
            _navController.ArriveDistance = _npc.arriveDistance;
            // Apply the current engage speed to the agent. Designers can
            // adjust the pursuit aggressiveness via the speed multiplier.
            float speed = _stats.GetEngageSpeed() * _origin.speedMultiplier;
            _navController.SetSpeed(speed);
            // Set the initial destination to the player's current position
            TrySetDestinationOnNavMesh((Vector2)_playerTransformAnchor.Value.position);
            _useNavMesh = true;
        }
    }

    public override void OnUpdate()
    {
        // Only move if nonIdle is true. When false, movement has been halted
        // by another action (e.g. arriving at the target) and we should not
        // process further movement until reactivated.
        if (!_npc.nonIdle)
            return;

        // Determine the player's current position each frame as it may change
        Vector2 playerPos = _playerTransformAnchor.Value.position;

        // Reset NavMesh usage each frame. We'll enable it if conditions below
        // indicate that we can use the NavMesh.
        _useNavMesh = false;

        // If a valid NavMeshAgentController is available and navigation was
        // configured on state entry, delegate movement to the NavMesh. The
        // controller will move the underlying Rigidbody2D along the computed
        // path and handle acceleration, deceleration and obstacle avoidance.
        if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        {
            // Mark that we are actively using the NavMesh this frame
            _useNavMesh = true;
            // Keep the arrival threshold in sync with the NPC's arrive distance
            _navController.ArriveDistance = _npc.arriveDistance;
            // Update the agent's speed each frame in case stats change
            float speed = _stats.GetEngageSpeed() * _origin.speedMultiplier;
            _navController.SetSpeed(speed);
            // Compute the desired destination in 3D space corresponding to
            // the player's current 2D position. Only update the destination
            // when it has changed significantly to avoid redundant calls.
            Vector3 currentDest = _navController.Agent.destination;
            Vector3 desiredDest = new Vector3(playerPos.x, playerPos.y, _navController.Agent.transform.position.z);
            if ((currentDest - desiredDest).sqrMagnitude > speed)
            {
                TrySetDestinationOnNavMesh(playerPos);
            }
            // Check whether we have reached the destination. Once the path
            // completes, halt movement and reset nonIdle so that other
            // behaviours (e.g. attack or idle) can take over.
            if (_navController.HasReachedDestination())
            {
                _navController.Stop();
                _npc.nonIdle = false;
                return;
            }
            // When using the NavMesh we do not perform manual velocity or
            // facing updates; the NavMeshAgentController handles those.
            return;
        }

        // Fallback: direct movement towards the player when no NavMesh is
        // available. Compute vector from enemy to player.
        Vector2 enemyPos = _npc.transform.position;
        Vector2 toPlayer = playerPos - enemyPos;
        float arriveSqr = _npc.arriveDistance * _npc.arriveDistance;
        // If we have arrived at the player (within arriveDistance), stop
        if (toPlayer.sqrMagnitude <= arriveSqr)
        {
            _movement.SetVelocityZero();
            _npc.nonIdle = false;
            return;
        }
        // Normalise direction and scale by engage speed and multiplier
        Vector2 dir = toPlayer.normalized;
        float runSpeed = _stats.GetEngageSpeed() * _origin.speedMultiplier;
        _movement.SetVelocity(dir * runSpeed);
        // Update facing direction to look at the player. The Movement
        // component does not update facing automatically, so adjust it here.
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            _movement.FacingDirection = dir.x >= 0 ? FacingDir.Right : FacingDir.Left;
        }
        else
        {
            _movement.FacingDirection = dir.y >= 0 ? FacingDir.Up : FacingDir.Down;
        }
    }

    public override void OnStateExit()
    {
        // When exiting, stop movement regardless of the mode. If navigation
        // was active, instruct the nav controller to stop and clear any
        // existing path. Then zero the velocity. Reset nonIdle so that
        // subsequent states begin in an idle state.
        if (_useNavMesh && _navController != null)
        {
            _navController.Stop();
        }
        _movement.SetVelocityZero();
        _npc.nonIdle = false;
    }

    /// <summary>
    /// Attempts to set the NavMeshAgentController's destination to the given
    /// 2D point. The point is converted into 3D world space and snapped to
    /// the nearest position on the NavMesh within a small radius. If no
    /// NavMesh is found near the target, a warning is logged.
    /// </summary>
    /// <param name="target2D">The desired 2D destination (x,y).</param>
    /// <returns>True if a valid destination was set; false otherwise.</returns>
    private bool TrySetDestinationOnNavMesh(Vector2 target2D)
    {
        if (_navController == null || _navController.Agent == null)
            return false;

        var agent = _navController.Agent;
        // Convert 2D coordinates to a 3D point. In a top‑down setup Z is
        // maintained at the agent's current Z to avoid undesirable vertical
        // movement.
        Vector3 dest = new Vector3(target2D.x, target2D.y, agent.transform.position.z);
        // Snap to the nearest point on the NavMesh within a reasonable range.
        if (NavMesh.SamplePosition(dest, out var hit, 5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
            return true;
        }
        Debug.LogWarning($"EngageMovementAction: Target not near NavMesh: {dest} (target={target2D})");
        return false;
    }
}