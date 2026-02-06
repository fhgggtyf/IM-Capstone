using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using Moment = Domicile.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Enemy/Set Animator Parameter")]
public class EnemyIdleMovingAnimatorParameterActionSO : StateActionSO
{
    public Moment whenToRun = default;
    public float playSpeed = 1;

    protected override StateAction CreateAction() => new EnemyIdleMovingAnimatorParameterAction(playSpeed);

}
public class EnemyIdleMovingAnimatorParameterAction : StateAction
{
    private AnimationManager _animManager;
    private NonPlayerCharacter _npc;
    private EnemyIdleMovingAnimatorParameterActionSO _originSO => (EnemyIdleMovingAnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
    private string[] _animName;
    private float _playSpeed;

    public EnemyIdleMovingAnimatorParameterAction( float playSpeedParam)
    {
        _playSpeed = playSpeedParam;
    }

    public override void Awake(StateMachine stateMachine)
    {
        _animManager = stateMachine.GetComponent<AnimationManager>();
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
    }

    public override void OnStateEnter()
    {
        if (_originSO.whenToRun == SpecificMoment.OnStateEnter)
            SetParameter();
    }

    public override void OnStateExit()
    {
        if (_originSO.whenToRun == SpecificMoment.OnStateExit)
            SetParameter();
    }

    private void SetParameter()
    {
        if(_npc.nonIdle)
            _animManager.ChangeAnimState("Walk", _playSpeed);
        else
            _animManager.ChangeAnimState("Idle", _playSpeed);
    }

    public override void OnUpdate() {
        if (_originSO.whenToRun == SpecificMoment.OnUpdate)
            SetParameter();
    }

}
