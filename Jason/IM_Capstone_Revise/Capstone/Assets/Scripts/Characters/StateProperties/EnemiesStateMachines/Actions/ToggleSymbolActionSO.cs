using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle", menuName = "State Machines/Actions/Enemies/SymbolToggle")]
public class ToggleSymbolActionSO : StateActionSO<ToggleSymbolAction>
{
    
}

public class ToggleSymbolAction : StateAction
{
    private NonPlayerCharacter _npc;
    private EnemySymbolController _enemySymbolController;
    private bool _lastNonIdleState;

    public override void Awake(StateMachine stateMachine)
    {
        base.Awake(stateMachine);
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _enemySymbolController = _npc.Core.GetCoreComponent<EnemySymbolController>();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _enemySymbolController.ToggleSusSymbol(true);
        _enemySymbolController.ToggleAlertSymbol(false);
        _lastNonIdleState = _npc.nonIdle;
    }

    public override void OnUpdate()
    {
        if (!_npc.nonIdle && _lastNonIdleState != _npc.nonIdle)
        {
            _enemySymbolController.ToggleSusSymbol(true);
            _enemySymbolController.ToggleAlertSymbol(false);
            _lastNonIdleState= _npc.nonIdle;
        }
        else if (_npc.nonIdle && _lastNonIdleState != _npc.nonIdle) 
        {
            _enemySymbolController.ToggleSusSymbol(false);
            _enemySymbolController.ToggleAlertSymbol(true);
            _lastNonIdleState=_npc.nonIdle;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        _enemySymbolController.ToggleSusSymbol(false);
        _enemySymbolController.ToggleAlertSymbol(false);
    }

}
