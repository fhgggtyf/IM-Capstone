using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "EntityConfig/Stats Config")]
public class StatsConfigSO : ScriptableObject
{
    [SerializeField] private int _initialActualHealth;
    [SerializeField] private int _initialActualArmor;
    [SerializeField] private int _initialActualMagicResist;
    [SerializeField] private int _initialActualAttack;
    [SerializeField] private int _initialActualAbilityPower;
    [SerializeField] private int _initialActualAttackSpeed;
    [SerializeField] private int _initialActualMana;
    [SerializeField] private int _initialActualTenacity;
    [SerializeField] private int _initialActualStamina;
    [SerializeField] private int _initialActualLuck;

    [SerializeField] private int _initialActualCooldown;
    [SerializeField] private float _initialCritChance;
    [SerializeField] private float _initialCritDamage;
    [SerializeField] private int _initialJumpCount;

    public int InitialHealth { get => _initialActualHealth; set => _initialActualHealth = value; }
    public int InitialArmor { get => _initialActualArmor; set => _initialActualArmor = value; }
    public int InitialMagicResist { get => _initialActualMagicResist; set => _initialActualMagicResist = value; }
    public int InitialAttack { get => _initialActualAttack; set => _initialActualAttack = value; }
    public int InitialAbilityPower { get => _initialActualAbilityPower; set => _initialActualAbilityPower = value; }
    public int InitialCooldown { get => _initialActualCooldown; set => _initialActualCooldown = value; }
    public int InitialMana { get => _initialActualMana; set => _initialActualMana = value; }
    public int InitialTenacity { get => _initialActualTenacity; set => _initialActualTenacity = value; }
    public int InitialStamina { get => _initialActualStamina; set => _initialActualStamina = value; }
    public int InitialLuck { get => _initialActualLuck; set => _initialActualLuck = value; }
    public float InitialCritChance { get => _initialCritChance; set => _initialCritChance = value; }
    public float InitialCritDamage { get => _initialCritDamage; set => _initialCritDamage = value; }
    public int InitialJumpCount { get => _initialJumpCount; set => _initialJumpCount = value; }
    public int InitialAttackSpeed { get => _initialActualAttackSpeed; set => _initialActualAttackSpeed = value; }

    public void InitializeBaseStats(StatBlock baseStats)
    {
        InitialHealth = baseStats.Health.Num;
        InitialArmor = baseStats.Armor.Num;
        InitialMagicResist = baseStats.MagicResist.Num;
        InitialAttack = baseStats.Attack.Num;
        InitialAbilityPower = baseStats.AbilityPower.Num;
        InitialAttackSpeed = baseStats.AttackSpeed.Num;
        InitialMana = baseStats.Mana.Num;
        InitialTenacity = baseStats.Tenacity.Num;
        InitialStamina = baseStats.Stamina.Num;
        InitialLuck = baseStats.Luck.Num;
    }

}
