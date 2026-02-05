using UnityEngine;
using UnityEngine.AI;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Action that moves the enemy towards the last heard position recorded on the
/// NonPlayerCharacter. This action should be used during the investigation
/// state after the idle/confusion phase has completed. Movement will continue
/// until the enemy arrives within its arriveDistance threshold. Upon arrival
/// movement stops and nonIdle is reset so that the next idle action (pause)
/// can execute. The speed of movement is taken from the NonPlayerStatsManager.
/// </summary>
[CreateAssetMenu(fileName = "InvestigateMovementAction", menuName = "State Machines/Actions/Enemies/Investigate Move")]
public class InvestigateMovementActionSO : StateActionSO<InvestigateMovementAction>
{
    // When investigating it is common for enemies to move faster than when
    // patrolling. If needed you can add a multiplier here to scale the
    // base running speed without modifying StatsConfigSO values.
    public float speedMultiplier = 1f;
}

public class InvestigateMovementAction : StateAction
{
    private NonPlayerCharacter _npc;
    private NonPlayerStatsManager _stats;
    private Movement _movement;
    private InvestigateMovementActionSO _origin;

    // Navigation controller for moving along a NavMesh. When present the
    // enemy will follow a computed path instead of moving directly to the
    // investigate target. This improves movement in complex environments.
    private NavMeshAgentController _navController;
    // Flag indicating whether navigation was engaged. Determines whether
    // to delegate movement to the nav controller or use direct movement.
    private bool _useNavMesh;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _stats = stateMachine.GetComponent<NonPlayerStatsManager>();
        _movement = _npc.Core.GetCoreComponent<Movement>();
        _origin = (InvestigateMovementActionSO)OriginSO;

        // Attempt to locate a NavMeshAgentController on the core. If it exists
        // and the agent is on a baked NavMesh then we can use navigation.
        _navController = _npc.Core.GetCoreComponent<NavMeshAgentController>();
    }

    public override void OnStateEnter()
    {
        //// On entering movement, ensure we are in nonIdle state so movement begins.
        //_npc.nonIdle = true;
        // Copy the last heard position into moveTarget to ensure we go to the right spot
        _npc.moveTarget = _npc.lastHeardPosition;

        // Reset navigation usage on state entry. We'll attempt to initialise
        // navigation if the agent is on a valid NavMesh.
        _useNavMesh = false;
        //if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        //{
        //    // Align arrival threshold with NPC configuration
        //    _navController.ArriveDistance = _npc.arriveDistance;
        //    // Apply investigate speed scaled by the optional multiplier
        //    float speed = _stats.GetInvestigateSpeed() * _origin.speedMultiplier;
        //    _navController.SetSpeed(speed);
        //    // Set the destination to the investigate target
        //    TrySetDestinationOnNavMesh(_npc.moveTarget);
        //    _useNavMesh = true;
        //}
    }

    public override void OnUpdate()
    {
        // Only update movement if nonIdle is true. When false, the NPC has
        // completed movement and should pause or perform another action.
        if (!_npc.nonIdle)
            return;

        // Reset NavMesh usage each frame. We'll set this flag to true when
        // using the NavMesh below. When false, fallback direct movement is used.
        _useNavMesh = false;

        // If navigation is available, delegate movement to the NavMesh agent.
        if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        {
            // Mark that NavMesh is being used this frame
            _useNavMesh = true;
            // Keep arrival threshold synced with NPC's arriveDistance
            _navController.ArriveDistance = _npc.arriveDistance;
            // Update agent speed to track any stat changes
            float speed = _stats.GetInvestigateSpeed() * _origin.speedMultiplier;
            _navController.SetSpeed(speed);
            // Update destination only when the target has changed significantly
            Vector3 currentDest = _navController.Agent.destination;
            Vector3 desiredDest = new Vector3(_npc.moveTarget.x, _npc.moveTarget.y, _navController.Agent.transform.position.z);
            if ((currentDest - desiredDest).sqrMagnitude > 0.01f)
            {
                TrySetDestinationOnNavMesh(_npc.moveTarget);
            }
            // Check if we have reached the destination
            if (_navController.HasReachedDestination())
            {
                _navController.Stop();
                _npc.nonIdle = false;
                return;
            }
            // When using NavMesh, manual velocity updates are not required
            return;
        }

        // Fallback: direct movement to the investigate target
        Vector2 currentPos = _npc.transform.position;
        Vector2 toTarget = _npc.moveTarget - currentPos;
        // If we have arrived at the target, stop and reset nonIdle
        if (toTarget.sqrMagnitude <= _npc.arriveDistance * _npc.arriveDistance)
        {
            _movement.SetVelocityZero();
            _npc.nonIdle = false;
            return;
        }
        // Move directly towards the target at investigate speed
        Vector2 direction = toTarget.normalized;
        float runSpeed = _stats.GetInvestigateSpeed() * _origin.speedMultiplier;
        _movement.SetVelocity(direction * runSpeed);
    }

    public override void OnStateExit()
    {
        // When exiting, stop movement regardless of whether navigation was
        // used. Clear any existing path on the nav controller and zero
        // the velocity to ensure a clean transition.
        if (_useNavMesh && _navController != null)
        {
            _navController.Stop();
        }
        _movement.SetVelocityZero();
    }

    /// <summary>
    /// Attempts to set the NavMeshAgentController's destination to the given
    /// 2D investigate point. The position is converted to 3D world space and
    /// snapped to the nearest NavMesh within a small radius. Returns true
    /// if a valid destination is found and set.
    /// </summary>
    /// <param name="target2D">The desired investigate destination (x,y).</param>
    /// <returns>True if a valid destination was assigned; otherwise false.</returns>
    private bool TrySetDestinationOnNavMesh(Vector2 target2D)
    {
        if (_navController == null || _navController.Agent == null)
            return false;
        var agent = _navController.Agent;
        // Convert target into a 3D point using the agent's current Z coordinate
        Vector3 dest = new Vector3(target2D.x, target2D.y, agent.transform.position.z);
        if (NavMesh.SamplePosition(dest, out var hit, 5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
            return true;
        }
        Debug.LogWarning($"InvestigateMovementAction: Target not near NavMesh: {dest} (target={target2D})");
        return false;
    }
}