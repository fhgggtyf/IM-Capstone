using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that evaluates to true when the player is providing any directional input
/// beyond a configurable threshold. Use this to trigger transitions into walking or
/// running states from idle.
/// </summary>
[CreateAssetMenu(fileName = "IsMovingCondition", menuName = "State Machines/Conditions/Is Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
    // Threshold magnitude squared required to consider the player as moving. The default
    // of 0.01f avoids floating point jitter triggering movement when idle.
    public float threshold = 0.1f;
}

public class IsMovingCondition : Condition
{
    private Player _player;
    private IsMovingConditionSO _originSO => (IsMovingConditionSO)OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
    }

    protected override bool Statement()
    {
        Vector2 input = _player.InputVector;
        // Use the squared magnitude to avoid expensive square roots.
        return input.sqrMagnitude > _originSO.threshold * _originSO.threshold;
    }
}


///// <summary>
///// Condition that returns true when the player's running flag is set. The flag should be
///// toggled by input or by state actions when appropriate.
///// </summary>
//[CreateAssetMenu(fileName = "IsRunningCondition", menuName = "State Machines/Conditions/Is Running")]
//public class IsRunningConditionSO : StateConditionSO<IsRunningCondition>
//{
//}

//public class IsRunningCondition : Condition
//{
//    private Player _player;

//    public override void Awake(StateMachine stateMachine)
//    {
//        _player = stateMachine.GetComponent<Player>();
//    }

//    protected override bool Statement()
//    {
//        return _player.isRunning;
//    }
//}

///// <summary>
///// Condition that returns true when the player's crouching flag is set.
///// </summary>
//[CreateAssetMenu(fileName = "IsCrouchingCondition", menuName = "State Machines/Conditions/Is Crouching")]
//public class IsCrouchingConditionSO : StateConditionSO<IsCrouchingCondition>
//{
//}

//public class IsCrouchingCondition : Condition
//{
//    private Player _player;

//    public override void Awake(StateMachine stateMachine)
//    {
//        _player = stateMachine.GetComponent<Player>();
//    }

//    protected override bool Statement()
//    {
//        return _player.isCrouching;
//    }
//}
