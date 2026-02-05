using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "EntityConfig/Stats Config")]
public class StatsConfigSO : ScriptableObject
{

    [SerializeField] private int _patrolSpeed = default(int);
    [SerializeField] private int _investigateSpeed = default(int);
    [SerializeField] private int _engageSpeed = default(int);

    [SerializeField] private int _walkingSpeed = default(int);
    [SerializeField] private int _walkingSound = default(int);
    [SerializeField] private int _idleSound = default(int);

    [SerializeField] private int _soundThreshold = default(int);

    // Enemy vision cone range (in world units). Determines how far the enemy can see the player.
    [SerializeField] private float _visionRange = 5f;

    // Enemy vision cone angle (in degrees). Determines the field of view of the enemy.
    [SerializeField] private float _visionAngle = 90f;

    public int PatrolSpeed { get => _patrolSpeed; set => _patrolSpeed = value; }
    public int InvestigateSpeed { get => _investigateSpeed; set => _investigateSpeed = value; }
    public int EngageSpeed { get => _engageSpeed; set => _engageSpeed = value; }
    public int WalkingSpeed { get => _walkingSpeed; set => _walkingSpeed = value; }
    public int WalkingSound { get => _walkingSound; set => _walkingSound = value; }
    public int IdleSound { get => _idleSound; set => _idleSound = value; }
    public int SoundThreshold { get => _soundThreshold; set => _soundThreshold = value; }

    /// <summary>
    /// Gets or sets the maximum range (in world units) of the enemy's vision cone. This
    /// controls how far ahead the enemy can see a target. A value of 5 means the enemy
    /// can detect objects within a 5‑unit radius when unobstructed.
    /// </summary>
    public float VisionRange { get => _visionRange; set => _visionRange = value; }

    /// <summary>
    /// Gets or sets the field of view of the enemy's vision cone in degrees. This
    /// angle represents the full width of the cone; half of this value is used when
    /// calculating whether a target is within the cone relative to the enemy's facing
    /// direction. Typical values are between 60 and 120.
    /// </summary>
    public float VisionAngle { get => _visionAngle; set => _visionAngle = value; }
}
