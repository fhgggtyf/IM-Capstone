using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Action responsible for determining the next patrol target for an enemy. When the
/// enemy finishes an idle cycle (nonIdle = false) this action finds the closest
/// unvisited point in the assigned PatrolRoute and sets it as the move target. Once
/// all points have been visited in the current patrol loop the visited flags are
/// reset so the enemy can start a new loop. This action only sets the target and
/// does not initiate movement; movement is handled by a separate state action.
/// </summary>
[CreateAssetMenu(fileName = "PatrolNextPointAction", menuName = "State Machines/Actions/Enemies/Patrol Next Point")]
public class PatrolNextPointActionSO : StateActionSO<PatrolNextPointAction>
{
    // No additional configuration required. All logic resides in the action.
}

public class PatrolNextPointAction : StateAction
{
    private NonPlayerCharacter _npc;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
    }

    public override void OnStateEnter()
    {
        // Reset flag so a new target is assigned at the beginning of the patrol cycle.
    }

    public override void OnUpdate()
    {
        // Only choose a new target when the NPC is idle (nonIdle == false). During
        // movement or investigation nonIdle will be true and we avoid reassigning the target.
        if (_npc.nonIdle || _npc.TargetAssigned)
        {
            Debug.Log("Either nonIdle true or target already assigned, so not picking new patrol point");
            return;
        }

        if (_npc.patrolRoute == null || _npc.patrolRoute.Points == null || _npc.patrolRoute.Points.Length == 0)
        {
            Debug.LogWarning("PatrolNextPointAction: No patrol route or points defined.");
            return;
        }

        var points = _npc.patrolRoute.Points;
        int n = points.Length;
        // Ensure the checkedIndices array is properly initialised
        if (_npc.checkedIndices == null || _npc.checkedIndices.Length != n)
        {
            _npc.checkedIndices = new int[n];
        }

        // Attempt to find the closest unvisited point
        int bestIndex = -1;
        float bestDist = float.MaxValue;
        Vector2 current = _npc.transform.position;
        for (int i = 0; i < n; i++)
        {
            if (_npc.checkedIndices[i] != 0) continue;
            Vector2 candidate = points[i].position;
            float dist = (candidate - current).sqrMagnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                bestIndex = i;
            }
        }

        // If all points were visited, reset the visited markers and search again
        if (bestIndex == -1)
        {
            for (int i = 0; i < n; i++)
                _npc.checkedIndices[i] = 0;

            // Re-run the search
            for (int i = 0; i < n; i++)
            {
                if (_npc.checkedIndices[i] != 0) continue;
                Vector2 candidate = points[i].position;
                float dist = (candidate - current).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestIndex = i;
                }
            }
        }

        // Assign target if found
        if (bestIndex != -1)
        {
            _npc.moveTarget = points[bestIndex].position;
            _npc.checkedIndices[bestIndex] = 1;
            _npc.TargetAssigned = true;

            // Mark the NPC as nonIdle so that the movement action begins moving
            //_npc.nonIdle = true;
        }
    }

    public override void OnStateExit()
    {
        // Clear flag on exit so a new cycle can begin
        _npc.TargetAssigned = false;
    }
}