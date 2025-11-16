using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsWalkingConditionSO : StateConditionSO<IsWalkingCondition>
{
    public float treshold = 0.02f;
}

public class IsWalkingCondition : Condition
{
    private Player _playerScript;
    private IsWalkingConditionSO _originSO => (IsWalkingConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void Awake(StateMachine stateMachine)
    {
        _playerScript = stateMachine.GetComponent<Player>();
    }

    protected override bool Statement()
    {
        Vector2 movementVector = _playerScript.InputVector;
        Debug.Log("Movement Vector: " + movementVector);
        movementVector.y = 0f;
        return movementVector.sqrMagnitude > _originSO.treshold;
    }
}
