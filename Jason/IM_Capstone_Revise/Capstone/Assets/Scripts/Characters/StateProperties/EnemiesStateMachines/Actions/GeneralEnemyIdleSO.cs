using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "EnemyIdle", menuName = "State Machines/Actions/Enemies/Idle With Timer")]
public class GeneralEnemyIdleSO : StateActionSO<GeneralEnemyIdle>
{
    public float idleDuration = 1.5f;
}

public class GeneralEnemyIdle : StateAction
{
    private NonPlayerCharacter _npc;
    private Movement _movement;

    private float _timer;
    private float _duration;

    private GeneralEnemyIdleSO Origin => (GeneralEnemyIdleSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _movement = _npc.Core.GetCoreComponent<Movement>();
    }

    public override void OnStateEnter()
    {
        _duration = Origin.idleDuration;
        _timer = 0f;

        // entering idle: freeze immediately
        _npc.nonIdle = false;
        _movement.SetVelocityZero();
    }

    public override void OnUpdate()
    {
        // If something else is driving movement, do nothing
        if (_npc.nonIdle)
        {
            Debug.Log("nonIdle true, so not idling");
            return;
        }

        // Ensure we stay stopped while idling
        _movement.SetVelocityZero();

        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            // release control so a move action can start moving
            _npc.nonIdle = true;
            _timer = 0f;
        }
    }

    public override void OnStateExit()
    {
        // Always stop cleanly on exit to avoid “carryover” velocity
        _movement.SetVelocityZero();
        _npc.nonIdle = false;
    }
}
