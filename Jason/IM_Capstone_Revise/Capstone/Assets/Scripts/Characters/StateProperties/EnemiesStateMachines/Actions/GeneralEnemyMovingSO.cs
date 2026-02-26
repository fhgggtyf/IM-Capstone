using System;
using System.Collections.Generic;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Moving", menuName = "State Machines/Actions/Enemies/Moving")]
public class GeneralEnemyMovingSO : StateActionSO<GeneralEnemyMoving> { }

/// <summary>
/// Default movement action for patrolling enemies. When a NavTilemap and PathAgent
/// are available on the enemy's Core, this action computes a path to the
/// assigned moveTarget on state entry and then follows it during updates.
/// If no navigation components exist, it falls back to direct movement
/// towards the moveTarget as in the original implementation. Movement
/// stops when the enemy reaches its arriveDistance threshold.
/// </summary>
public class GeneralEnemyMoving : StateAction
{
    private NonPlayerCharacter _npc;
    private Movement _movement;
    private NonPlayerStatsManager _stats;
    private NavMeshAgentController _navController;
    private bool _useNavMesh;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _movement = _npc.Core.GetCoreComponent<Movement>();
        _stats = stateMachine.GetComponent<NonPlayerStatsManager>();
        // Attempt to locate a NavMeshAgentController on the Core. This component
        // wraps a NavMeshAgent and integrates it with the 2D Movement. If
        // present and the agent is on a baked NavMesh surface, we can use
        // navigation instead of simple straight‑line movement.
        _navController = _npc.Core.GetCoreComponent<NavMeshAgentController>();
    }

    public override void OnStateEnter()
    {
        _useNavMesh = false;
        // Initialise navigation on state entry if a controller and agent exist
        // and the agent is on the NavMesh. This check ensures we only use the
        // NavMesh when a walkable grid is available. Otherwise we fall back
        // to direct movement.
        if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        {
            // Align the arrival threshold with the NPC's arrive distance
            _navController.ArriveDistance = _npc.arriveDistance;
            // Apply the current patrol speed to the agent
            _navController.SetSpeed(_stats.GetPatrolSpeed());
            // Set the destination to the NPC's move target (x,y)
            _navController.SetDestination(_npc.moveTarget);
            _useNavMesh = true;
        }
        else
        {
            Debug.Log(_navController);
            Debug.Log(_navController.Agent);
            Debug.Log(_navController.Agent.isOnNavMesh);
            Debug.Log("NavMeshAgentController not available or agent not on NavMesh, falling back to direct movement");
        }
    }

    public override void OnUpdate()
    {
        // Only move once idle logic has released control
        if (!_npc.nonIdle)
        {
            Debug.Log("nonIdle false, so not moving");
            return;
        }
        // If a valid NavMeshAgentController is available and we configured navigation
        // on state entry, defer movement to the nav controller. The controller
        // updates the Rigidbody2D position and velocity via its Update.

        if (_navController != null && _navController.Agent != null && _navController.Agent.isOnNavMesh)
        {
            // Align the arrival threshold with the NPC's arrive distance
            _navController.ArriveDistance = _npc.arriveDistance;
            // Apply the current patrol speed to the agent
            _navController.SetSpeed(_stats.GetPatrolSpeed());

            var a = _navController.Agent;
            //Debug.Log($"agentPos={a.transform.position} destBeforeSnap={_npc.moveTarget} agentDest={a.destination} remaining={a.remainingDistance}");
            // Set the destination to the NPC's move target (x,y)
            TrySetDestinationOnNavMesh(_npc.moveTarget);

            _useNavMesh = true;
        }

        if (_useNavMesh && _navController != null)
        {
            Debug.Log("Using NavMesh for movement");
            //Debug.Log($"hasPath={_navController.Agent.hasPath}, pathPending={_navController.Agent.pathPending}, remaining={_navController.Agent.remainingDistance}, status={_navController.Agent.pathStatus}");

            // Keep the speed in sync with the stats each frame
            _navController.SetSpeed(_stats.GetPatrolSpeed());
            // Update destination if the target has changed
            // Use approximately equality check to avoid redundant SetDestination calls
            if ((_navController.Agent.destination - (Vector3)_npc.moveTarget).sqrMagnitude > 0.01f)
            {
                TrySetDestinationOnNavMesh(_npc.moveTarget);

            }

            // Check for arrival using the nav controller's path status
            if (_navController.HasReachedDestination())
            {
                _navController.Stop();
                _npc.nonIdle = false;
                _npc.TargetAssigned = false;
            }
            return;
        }

        // Fallback: direct movement towards the target when no NavMesh is available
        Vector2 currentPos = _npc.transform.position;
        Vector2 toTarget = _npc.moveTarget - currentPos;
        // Arrived at target
        if (toTarget.sqrMagnitude <= _npc.arriveDistance * _npc.arriveDistance)
        {
            _movement.SetVelocityZero();
            _npc.nonIdle = false;
            _npc.TargetAssigned = false;
            return;
        }

        Vector2 direction = toTarget.normalized;
        float speed = _stats.GetPatrolSpeed();
        _movement.SetVelocity(direction * speed);
    }

    public override void OnStateExit()
    {
        // When exiting, stop movement regardless of the mode. If navigation
        // was active, reset the agent to ensure no residual path remains. If
        // falling back to movement, simply zero the velocity.
        if (_useNavMesh && _navController != null)
        {
            _navController.Stop();
        }
        _movement.SetVelocityZero();
        _npc.nonIdle = false;
        _npc.TargetAssigned = false;
    }

    private bool TrySetDestinationOnNavMesh(Vector2 target2D)
    {
        if (_navController == null || _navController.Agent == null) return false;

        var agent = _navController.Agent;

        // Convert 2D target (x,y) into a 3D point consistent with your world.
        // In many 2D top-down setups, Y is "up" in world, and movement happens in X/Y with Z fixed.
        Vector3 dest = new Vector3(target2D.x, target2D.y, agent.transform.position.z);

        // Snap to nearest NavMesh point near that destination
        if (NavMesh.SamplePosition(dest, out var hit, 5f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
            return true;
        }

        Debug.LogWarning($"Target not near NavMesh: {dest} (moveTarget={target2D})");
        return false;
    }
}