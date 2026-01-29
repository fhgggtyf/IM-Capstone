using System;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Horizontal", menuName = "State Machines/Actions/Enemies/Idle Horizontal With Timer")]
public class GeneralEnemyIdleSO : StateActionSO<GeneralEnemyIdle>
{
    public float idleDuration = 1.5f;
}


public class GeneralEnemyIdle : StateAction
{
    private NonPlayerCharacter _npc;
    private EnemySignals _signals;

    private float _timer;
    private float _duration;

    private GeneralEnemyIdleSO Origin => (GeneralEnemyIdleSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _signals = stateMachine.GetComponent<EnemySignals>(); //  per enemy
    }

    public override void OnStateEnter()
    {
        _duration = Origin.idleDuration;
        _timer = 0f;

        _signals.IdleFinished += OnIdleFinished;
    }

    public override void OnUpdate()
    {
        if (_npc.nonIdle)
            return;

        _npc.movementVector = Vector2.zero;

        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            _signals.RaiseIdleFinished(); // safe
        }
    }

    public override void OnStateExit()
    {
        _signals.IdleFinished -= OnIdleFinished;
        _npc.nonIdle = false;
    }

    private void OnIdleFinished()
    {
        _npc.nonIdle = true;
    }
}
