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


}
