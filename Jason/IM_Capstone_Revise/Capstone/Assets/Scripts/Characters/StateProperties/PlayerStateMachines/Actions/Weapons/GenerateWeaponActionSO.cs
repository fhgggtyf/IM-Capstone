using UnityEngine;
using UnityEngine.Events;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GenerateWeaponAction", menuName = "State Machines/Actions/GenerateWeapon")]
public class GenerateWeaponActionSO : StateActionSO
{
    public CombatInputs weaponIndex;
    protected override StateAction CreateAction() => new GenerateWeaponAction(weaponIndex);
}

public class GenerateWeaponAction : StateAction
{
    private CombatInputs _weaponIndex;

    private Player _player;
    private Weapon _weapon;
    private WeaponGenerator _weaponGenerator;

    private PlayerStatsManager _statsManager;
    public GenerateWeaponAction(CombatInputs weaponIndex)
    {
        _weaponIndex = weaponIndex;
    }

    public override void Awake(StateMachine stateMachine)
    {
        _player = stateMachine.GetComponent<Player>();
        _statsManager = stateMachine.GetComponent<PlayerStatsManager>();
    }

    public override void OnStateEnter()
    {
        _weapon = _player.weapons[(int)_weaponIndex];
        _weaponGenerator = _weapon.GetComponent<WeaponGenerator>();
        _weapon.EventHandler.OnFinish += HandleFinish;
        _weapon.OnUseInput += HandleUseInput;

        _weapon.Enter(_statsManager.GetCurrentAttackSpeed());
    }

    private void HandleFinish()
    {
        _player.isAttackFinished = true;
    }

    public override void OnUpdate()
    {

    }

    public override void OnStateExit()
    {
        _weapon.Exit();
    }

    private void HandleUseInput() => _player.UseAttackInput((int)_weaponIndex);
}
