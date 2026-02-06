using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using Moment = Domicile.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
public class AnimatorParameterActionSO : StateActionSO
{
    public string animName = default;
    public Moment whenToRun = default;
    public float playSpeed = 1;

    protected override StateAction CreateAction() => new AnimatorParameterAction(animName, playSpeed);

}
public class AnimatorParameterAction : StateAction
{
    private AnimationManager _animManager;
    private AnimatorParameterActionSO _originSO => (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
    private string _animName;
    private float _playSpeed;

    public AnimatorParameterAction(string param, float playSpeedParam)
    {
        _animName = param;
        _playSpeed = playSpeedParam;
    }

    public override void Awake(StateMachine stateMachine)
    {
        _animManager = stateMachine.GetComponent<AnimationManager>();
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
        _animManager.ChangeAnimState(_animName, _playSpeed);
    }

    public override void OnUpdate() {
        if (_originSO.whenToRun == SpecificMoment.OnUpdate)
            SetParameter();
    }

}
