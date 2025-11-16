using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerStatsManager : StatsManager
{
    //[SerializeField] private StatsConfigSO statsConfig = default;
    //[SerializeField] private IngameStatsSO currentStatsSO;

    //public StatsConfigSO StatsConfig { get => statsConfig; set => statsConfig = value; }
    //public IngameStatsSO CurrentStatsSO { get => currentStatsSO; set => currentStatsSO = value; }

    public void OnEnable()
    {
        //If the HealthSO hasn't been provided in the Inspector (as it's the case for the player),
        //we create a new SO unique to this instance of the component. This is typical for enemies.
        if (CurrentStatsSO == null)
        {
            CurrentStatsSO = ScriptableObject.CreateInstance<IngameStatsSO>();
            InitializeStats();
        }
    }

}
