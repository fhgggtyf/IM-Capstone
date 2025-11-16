using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDamageable
{
    [Header("stats")]
    [SerializeField] private StatsManager _statsManager;

    //[Header("Combat")]
    //[SerializeField] private GetHitEffectConfigSO _getHitEffectSO;
    //[SerializeField] private Renderer _mainMeshRenderer;
    //[SerializeField] private DroppableRewardConfigSO _droppableRewardSO;

    [Header("Broadcasting On")]
    [SerializeField] public VoidEventChannelSO updateHealthBar = default;
    [SerializeField] private VoidEventChannelSO _deathEvent = default;

    [Header("Listening To")]
    [SerializeField] private IntEventChannelSO _restoreHealth = default; //Getting cured when eating food

    //public DroppableRewardConfigSO DroppableRewardConfig => _droppableRewardSO;

    private StatsConfigSO _healthConfigSO;
    private IngameStatsSO _currentStatsSO;

    //Flags that the StateMachine uses for Conditions to move between states
    public bool GetHit { get; set; }
    public bool IsDead { get; set; }
    public bool Invincible { get => invincible; set => invincible = value; }

    [SerializeField] private bool invincible;

    [NonSerialized] public DamageData lastHitData;

    //public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
    //public Renderer MainMeshRenderer => _mainMeshRenderer; //used to apply the hit flash effect

    public event UnityAction OnDie;

    private void Awake()
    {

        if (updateHealthBar != null)
            updateHealthBar.RaiseEvent();
        else
        {
            updateHealthBar = ScriptableObject.CreateInstance<VoidEventChannelSO>();
        }
    }

    private void Start()
    {
        _healthConfigSO = _statsManager.StatsConfig;
        _currentStatsSO = _statsManager.CurrentStatsSO;

        if (_restoreHealth != null)
            _restoreHealth.OnEventRaised += Cure;
    }

    private void OnDisable()
    {
        if (_restoreHealth != null)
            _restoreHealth.OnEventRaised -= Cure;
    }

    public void Damage(DamageData data)
    {
        lastHitData = data;

        if (IsDead)
            return;

        float damage = CalculateActualDamage(data);

        _currentStatsSO.InflictDamage((int)damage);

        if (updateHealthBar != null)
            updateHealthBar.RaiseEvent();

        GetHit = true;
        Debug.Log(_currentStatsSO.CurrentHealth);
        if (_currentStatsSO.CurrentHealth <= 0)
        {
            IsDead = true;

            //if (OnDie != null)
            //    OnDie.Invoke();

            //if (_deathEvent != null)
            //    _deathEvent.RaiseEvent();

            //_currentStatsSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
        }
    }

    private float CalculateActualDamage(DamageData data)
    {
        switch (data.AbilityParam.type)
        {
            case DamageType.Physical:
                int equivArmor = _currentStatsSO.CurrentArmor - data.ArmorIgnore;
                return data.Amount * (1f - (equivArmor / (100f + equivArmor)));
            case DamageType.Magical:
                int equivMR = _currentStatsSO.CurrentMR - data.MrIgnore;
                return data.Amount * (1f - (equivMR / (100f + equivMR)));
            case DamageType.True:
                return data.Amount;
        }
        return 0;
    }

    public void Kill()
    {
        Damage(new DamageData(_currentStatsSO.CurrentHealth, 0, 0, ScriptableObject.CreateInstance<AbilityDataSO>(), EnemyType.None, null));
    }

    /// <summary>
    /// Called by the StateMachine action ResetHealthSO. Used to revive the Rock critters.
    /// </summary>
    public void Revive()
    {
        _currentStatsSO.SetCurrentHealth(_healthConfigSO.InitialHealth);

        if (updateHealthBar != null)
            updateHealthBar.RaiseEvent();

        IsDead = false;
    }

    /// <summary>
    /// Used for cure events, like eating food. Triggered by an IntEventChannelSO.
    /// </summary>
    private void Cure(int healthToAdd)
    {
        if (IsDead)
            return;

        _currentStatsSO.RestoreHealth(healthToAdd);

        if (updateHealthBar != null)
            updateHealthBar.RaiseEvent();
    }
}
