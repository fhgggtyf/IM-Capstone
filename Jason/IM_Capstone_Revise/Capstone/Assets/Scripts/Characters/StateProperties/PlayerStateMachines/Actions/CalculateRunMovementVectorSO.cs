using System;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalRun", menuName = "State Machines/Actions/Horizontal Run")]
public class CalculateRunMovementVectorSO : StateActionSO<CalculateRunMovementVector>
{
    [Tooltip("Horizontal X plane speed multiplier")]
    public float speed = 4f;

    public MoveState moveState;
}
public class CalculateRunMovementVector : StateAction
{

    private Player _player;

    private float _runMultiplier;
    private CalculateRunMovementVectorSO _originSO => (CalculateRunMovementVectorSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    public override void OnStateEnter()
    {
        switch (_originSO.moveState)
        {
            case MoveState.Walk:
                _runMultiplier = 1;
                break;
            case MoveState.Run:
                _runMultiplier = 1.6f;
                break;
            case MoveState.Crouch:
                _runMultiplier = 0.6f;
                break;
        }
    }
    public override void OnUpdate()
    {
        _player.movementVector.x = _player.InputVector.x * _originSO.speed * _runMultiplier;
        _player.movementVector.y = _player.InputVector.y * _originSO.speed * _runMultiplier;
    }
}

public enum MoveState
{
    Walk,
    Run,
    Crouch
}
