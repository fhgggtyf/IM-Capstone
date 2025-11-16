using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// An instance of the Stats of a character, be it the player or an NPC.
/// The initial values are usually contained in another SO of type statsConfigSO.
/// </summary>
[CreateAssetMenu(fileName = "PlayersIngameStats", menuName = "EntityConfig/Player's stats")]
public class IngameStatsSO : ScriptableObject
{

    [Tooltip("Stats")]
    [SerializeField] [ReadOnly] private int _maxHealth;
    [SerializeField] [ReadOnly] private int _currentHealth;
    [SerializeField] [ReadOnly] private int _currentArmor;
    [SerializeField] [ReadOnly] private int _currentMR;
    [SerializeField] [ReadOnly] private int _currentAttack;
    [SerializeField] [ReadOnly] private int _currentAP;
    [SerializeField] [ReadOnly] private float _currentAttackSpeed;
    [SerializeField] [ReadOnly] private int _currentCD;
    [SerializeField] [ReadOnly] private int _maxMana;
    [SerializeField] [ReadOnly] private int _currentMana;
    [SerializeField] [ReadOnly] private int _currentArmorIgnore;
    [SerializeField] [ReadOnly] private int _currentMRIgnore;
    [SerializeField] [ReadOnly] private int _currentTenacity;
    [SerializeField] [ReadOnly] private float _currentStamina;
    [SerializeField] [ReadOnly] private int _maxStamina;
    [SerializeField] [ReadOnly] private int _currentLuck;
    [SerializeField] [ReadOnly] private float _currentCritChance;
    [SerializeField] [ReadOnly] private float _currentCritDmg;
    [SerializeField] [ReadOnly] private int _currentMaxJumps;

    [Tooltip("Localization")]
    [SerializeField] public LocalizedString MaxHPLocale;
    [SerializeField] public LocalizedString ArmorLocale;
    [SerializeField] public LocalizedString MRLocale;
    [SerializeField] public LocalizedString AtkLocale;
    [SerializeField] public LocalizedString APLocale;
    [SerializeField] public LocalizedString TenacityLocale;
    [SerializeField] public LocalizedString StaminaLocale;
    [SerializeField] public LocalizedString LuckLocale;
    [SerializeField] public LocalizedString ArmorIgnoreLocale;
    [SerializeField] public LocalizedString MRIgnoreLocale;


    public int MaxHealth { get => _maxHealth; }
    public int CurrentHealth { get => _currentHealth; }
    public int CurrentArmor { get => _currentArmor; }
    public int CurrentMR { get => _currentMR; }
    public int CurrentAttack { get => _currentAttack; }
    public int CurrentAP { get => _currentAP; }
    public int CurrentCD { get => _currentCD; }
    public int MaxMana { get => _maxMana; }
    public int CurrentMana { get => _currentMana; }
    public int CurrentTenacity { get => _currentTenacity; }
    public float CurrentStamina { get => _currentStamina; }
    public int MaxStamina { get => _maxStamina; }
    public int CurrentLuck { get => _currentLuck; }
    public float CurrentCritChance { get => _currentCritChance; }
    public float CurrentCritDmg { get => _currentCritDmg; }
    public int CurrentMaxJumps { get => _currentMaxJumps; }
    public int CurrentArmorIgnore { get => _currentArmorIgnore; set => _currentArmorIgnore = value; }
    public int CurrentMRIgnore { get => _currentMRIgnore; set => _currentMRIgnore = value; }
    public float CurrentAttackSpeed { get => _currentAttackSpeed; set => _currentAttackSpeed = value; }

    public void SetMaxHealth(int newValue)
    {
        _maxHealth = newValue;
    }

    public void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void InflictDamage(int DamageValue)
    {
        _currentHealth = Mathf.Max(_currentHealth - DamageValue, 0);
    }

    public void RestoreHealth(int HealthValue)
    {
        _currentHealth += HealthValue;
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }

    public void SetCurrentArmor(int newValue)
    {
        _currentArmor = newValue;
    }

    public void SetCurrentMR(int newValue)
    {
        _currentMR = newValue;
    }

    public void SetCurrentAttack(int newValue)
    {
        _currentAttack = newValue;
    }

    public void SetCurrentAP(int newValue)
    {
        _currentAP = newValue;
    }

    public void SetCurrentCD(int newValue)
    {
        _currentCD = newValue;
    }

    public void SetCurrentAttackSpeed(float newValue)
    {
        _currentAttackSpeed = newValue;
    }

    public void SetCurrentMana(int newValue)
    {
        _currentMana = newValue;
    }
    public void SetCurrentTenacity(int newValue)
    {
        _currentTenacity = newValue;
    }

    public void SetCurrentStamina(int newValue)
    {
        _currentStamina = newValue;
    }

    public void SetMaxStamina(int newValue)
    {
        _maxStamina = newValue;
    }

    public void SpendStamina(float spentValue)
    {
        _currentStamina -= spentValue;
        if (_currentStamina <= 0)
        {
            _currentStamina = 0;
        }
    }

    public void RestoreStamina(float addValue)
    {
        _currentStamina += addValue;
        if (_currentStamina >= _maxStamina)
        {
            _currentStamina = _maxStamina;
        }
    }

    public void SetCurrentLuck(int newValue)
    {
        _currentLuck = newValue;
    }

    public void SetCurrentCritChance(float newValue)
    {
        _currentCritChance = newValue;
    }

    public void SetCurrentCritDmg(float newValue)
    {
        _currentCritDmg = newValue;
    }

    public void SetCurrentMaxJumps(int newValue)
    {
        _currentMaxJumps = newValue;
    }
}
