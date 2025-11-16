using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] protected StatsConfigSO statsConfig = default;
    [SerializeField] protected IngameStatsSO currentStatsSO = default;

    public StatsConfigSO StatsConfig { get => statsConfig; set => statsConfig = value; }
    public IngameStatsSO CurrentStatsSO { get => currentStatsSO; set => currentStatsSO = value; }

    protected virtual void InitializeStats()
    {
        CurrentStatsSO.SetMaxHealth(StatsConfig.InitialHealth);
        CurrentStatsSO.SetCurrentHealth(StatsConfig.InitialHealth);
        CurrentStatsSO.SetCurrentArmor(StatsConfig.InitialArmor);
        CurrentStatsSO.SetCurrentMR(StatsConfig.InitialMagicResist);
        CurrentStatsSO.SetCurrentAttack(StatsConfig.InitialAttack);
        CurrentStatsSO.SetCurrentAP(StatsConfig.InitialAbilityPower);
        CurrentStatsSO.SetCurrentTenacity(StatsConfig.InitialTenacity);
    }

    public int GetCurrentTenacity()
    {
        return currentStatsSO.CurrentTenacity;
    }
}
