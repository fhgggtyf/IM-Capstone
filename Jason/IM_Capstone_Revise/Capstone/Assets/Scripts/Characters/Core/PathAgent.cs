using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores and manages a series of waypoints representing a path for an
/// enemy to follow. The path is defined as a list of world positions and
/// the PathAgent maintains an index into this list as the current
/// waypoint. The agent can report whether a path is present, return
/// the current waypoint and advance the index when the enemy gets
/// sufficiently close to the waypoint. This component should be added
/// to the same GameObject as the enemy's Core component so that it can
/// be retrieved via <see cref="Core.GetCoreComponent{T}()"/>.
/// </summary>
public class PathAgent : CoreComponent
{
    private List<Vector2> _path;
    private int _currentIndex;

    /// <summary>
    /// Whether a path has been assigned and there are waypoints left to follow.
    /// </summary>
    public bool HasPath => _path != null && _currentIndex < _path.Count;

    /// <summary>
    /// Current waypoint in world coordinates. Only valid if <see cref="HasPath"/> is true.
    /// </summary>
    public Vector2 CurrentWaypoint => _path[_currentIndex];

    /// <summary>
    /// Assign a new path for this agent to follow. The provided list should
    /// already be in world coordinates. Resets the internal index to the
    /// first waypoint.
    /// </summary>
    public void SetPath(List<Vector2> waypoints)
    {
        _path = waypoints;
        _currentIndex = 0;
    }

    /// <summary>
    /// Clears the current path. After calling this, <see cref="HasPath"/> will be false.
    /// </summary>
    public void ClearPath()
    {
        _path = null;
        _currentIndex = 0;
    }

    /// <summary>
    /// Advance to the next waypoint if the agent is within the given
    /// threshold distance of the current waypoint. When all waypoints
    /// have been reached this has no effect.
    /// </summary>
    public void AdvanceIfReached(Vector2 agentPosition, float threshold)
    {
        if (!HasPath) return;
        if (Vector2.SqrMagnitude(CurrentWaypoint - agentPosition) <= threshold * threshold)
        {
            _currentIndex++;
        }
    }
}