using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/General/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
    private Character _character;

    protected Movement Movement
    {
        get => movement ?? _character.Core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    //public Rigidbody2D RB { get; private set; }

    public override void Awake(StateMachine stateMachine)
    {
        _character = stateMachine.GetComponent<Character>();
        //RB = stateMachine.GetComponent<Rigidbody2D>();
    }

    public override void OnUpdate()
    {
        Movement.SetVelocity(_character.movementVector);
        //_player.movementVector = RB.velocity;
    }
}


