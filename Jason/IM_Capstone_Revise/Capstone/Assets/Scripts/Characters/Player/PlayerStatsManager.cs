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

    [Header("Broadcast On")]
    [SerializeField] private IntEventChannelSO onPlayerNoiseRadiusChange = default;

    /// <summary>
    /// The current noise being produced by the entity. This value should be updated by
    /// state actions whenever the player's movement state changes or when noise should
    /// otherwise be modified (e.g. when holding breath). Other systems can query
    /// <see cref="CurrentNoise"/> to determine how loud the player currently is.
    /// </summary>
    private int currentNoise;

    /// <summary>
    /// Gets the current noise level produced by the entity.
    /// </summary>
    public int CurrentNoise => currentNoise;

    /// <summary>
    /// Explicitly sets the current noise. State actions should call this when entering
    /// or updating movement states to reflect the appropriate noise value.
    /// </summary>
    /// <param name="noise">The new noise value.</param>
    public void SetCurrentNoise(int noise)
    {
        currentNoise = noise;
        // Additional behaviour (e.g. broadcasting an event) could be hooked in here if needed.
        onPlayerNoiseRadiusChange.RaiseEvent(currentNoise);
    }

    /// <summary>
    /// Returns the walking speed configured in the StatsConfigSO.
    /// </summary>
    public float GetWalkingSpeed() => statsConfig != null ? statsConfig.WalkingSpeed : 0f;

    /// <summary>
    /// Returns the running speed configured in the StatsConfigSO.
    /// </summary>
    public float GetRunningSpeed() => statsConfig != null ? statsConfig.RunningSpeed : 0f;

    /// <summary>
    /// Returns the crouching speed configured in the StatsConfigSO.
    /// </summary>
    public float GetCrouchingSpeed() => statsConfig != null ? statsConfig.CrouchingSpeed : 0f;

    /// <summary>
    /// Returns the noise value associated with running from the StatsConfigSO.
    /// </summary>
    public int GetRunningNoise() => statsConfig != null ? statsConfig.RunningSound : 0;

    /// <summary>
    /// Returns the noise value associated with walking from the StatsConfigSO.
    /// </summary>
    public int GetWalkingNoise() => statsConfig != null ? statsConfig.WalkingSound : 0;

    /// <summary>
    /// Returns the noise value associated with crouching from the StatsConfigSO.
    /// </summary>
    public int GetCrouchingNoise() => statsConfig != null ? statsConfig.CrouchingSound : 0;

    /// <summary>
    /// Returns the noise value associated with idle from the StatsConfigSO.
    /// </summary>
    public int GetIdleNoise() => statsConfig != null ? statsConfig.IdleSound : 0;
}

