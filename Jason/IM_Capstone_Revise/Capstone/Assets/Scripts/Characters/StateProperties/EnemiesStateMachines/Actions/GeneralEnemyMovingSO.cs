using System;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Horizontal", menuName = "State Machines/Actions/Enemies/Horizontal")]
public class GeneralEnemyMovingSO : StateActionSO<GeneralEnemyMoving> { }

public class GeneralEnemyMoving : StateAction
{
    private NonPlayerCharacter _npc;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
    }

    public override void OnUpdate()
    {
        // Current position (top-down 2D => x,y)
        Vector2 pos = _npc.transform.position;
        //Vector2 toTarget = _npc.moveTarget - pos;

        // Optional: stop when close enough
        //if (toTarget.sqrMagnitude <= _npc.arriveDistance * _npc.arriveDistance)
        {
            _npc.movementVector = Vector2.zero;
            return;
        }

        //Vector2 dir = toTarget.normalized;

        // movementVector should usually be a direction (-1..1), NOT already multiplied by speed,
        // depending on how your Movement component applies speed.
        // If you DO want to bake speed here, multiply by BaseSpeed.
        //_npc.movementVector = dir;
    }
}