using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : StatsManager
{
    [SerializeField] private StatsConfigSO playerStatsConfig = default;

    /// <summary>
    /// Assign the stats configuration from the inspector to the base StatsManager field.
    /// Without this, calls to GetRunningSpeed(), GetWalkingSpeed(), etc. will fall back
    /// to default values on the StatsConfigSO.
    /// </summary>
    private void Awake()
    {
        statsConfig = playerStatsConfig;
    }
}

