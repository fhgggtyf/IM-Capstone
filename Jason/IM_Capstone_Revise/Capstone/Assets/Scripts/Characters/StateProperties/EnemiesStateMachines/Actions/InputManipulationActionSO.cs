using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// This action handles the behaviour while the player is hiding inside a hideable object.
/// When the hiding state is entered the player is repositioned to the hideable's
/// transform, movement is stopped and noise is muted. It also maintains the
/// isHiding flag on the player so that conditions can detect when the hiding state
/// should end.
/// </summary>
[CreateAssetMenu(fileName = "InputManipulateAction", menuName = "State Machines/Enemies/Actions/InputManipulation")]
public class InputManipulationActionSO : StateActionSO<InputManipulationAction>
{
    public InputReader _inputReader;
}

public class InputManipulationAction : StateAction
{

    private InputReader _inputReader;
    InputManipulationActionSO _originSO => (InputManipulationActionSO)base.OriginSO;

    public override void Awake(StateMachine stateMachine)
    {
        _inputReader = _originSO._inputReader;
    }

    public override void OnStateEnter()
    {
        _inputReader.DisableAllInput();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnStateExit()
    {
       
    }
}
