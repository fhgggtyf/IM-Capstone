using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class responsible for exposing movement speed and noise values from a StatsConfigSO.
/// This manager can be attached to an entity (e.g. the player) to provide easy access to
/// configurable speed and sound values. It also tracks the current noise being made by
/// the entity so that other systems (AI, UI, etc.) can query it.
/// </summary>
public class StatsManager : MonoBehaviour
{
    /// <summary>
    /// Configuration object containing movement speeds and sound levels for the different
    /// movement states. Assign this in the inspector on a concrete subclass (e.g.
    /// PlayerStatsManager).
    /// </summary>
    [SerializeField] protected StatsConfigSO statsConfig = default;

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
