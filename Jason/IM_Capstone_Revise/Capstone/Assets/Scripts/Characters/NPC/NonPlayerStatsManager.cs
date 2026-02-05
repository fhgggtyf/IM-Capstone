using System;
using UnityEngine;

public class NonPlayerStatsManager : StatsManager
{
    public event Action<int> SoundDetectionLevelChanged;

    private int soundDetectionLevel;

    public int SoundDetectionLevel
    {
        get => GetSoundThreshold();
        set
        {
            if (soundDetectionLevel == value) return;
            soundDetectionLevel = value;
            SoundDetectionLevelChanged?.Invoke(soundDetectionLevel);
        }
    }

    public int GetSoundThreshold()
    {
        return statsConfig != null ? statsConfig.SoundThreshold : 0;
    }

    /// <summary>
    /// Returns the walking speed configured in the StatsConfigSO for this enemy. If no
    /// configuration is present, zero is returned. Enemies will use this value when
    /// patrolling between points.
    /// </summary>
    public float GetPatrolSpeed() => statsConfig != null ? statsConfig.PatrolSpeed : 0f;

    /// <summary>
    /// Returns the running speed configured in the StatsConfigSO for this enemy. When
    /// investigating a player. this value should
    /// typically be higher than the walking speed to allow the enemy to close the
    /// distance quickly.
    /// </summary>
    public float GetInvestigateSpeed() => statsConfig != null ? statsConfig.InvestigateSpeed : 0f;

    /// <summary>
    /// Returns the engaging speed configured in the StatsConfigSO for this enemy. When
    /// chasing a player. this value should
    /// typically be higher than the Investigation speed to allow the enemy to close the
    /// distance quickly.
    /// </summary>
    public float GetEngageSpeed() => statsConfig != null ? statsConfig.EngageSpeed : 0f;

    /// <summary>
    /// Returns the range of the enemy's vision cone in world units. This is exposed
    /// through the StatsConfigSO so that designers can tweak visibility without
    /// editing code.
    /// </summary>
    public float GetVisionRange() => statsConfig != null ? statsConfig.VisionRange : 0f;

    /// <summary>
    /// Returns the full field of view angle of the enemy's vision cone in degrees.
    /// </summary>
    public float GetVisionAngle() => statsConfig != null ? statsConfig.VisionAngle : 0f;
}
