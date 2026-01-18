using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "EntityConfig/Stats Config")]
public class StatsConfigSO : ScriptableObject
{

    [SerializeField] private int _walkingSpeed = default(int);
    [SerializeField] private int _runningSpeed = default(int);
    [SerializeField] private int _crouchingSpeed = default(int);

    [SerializeField] private int _runningSound = default(int);
    [SerializeField] private int _walkingSound = default(int);
    [SerializeField] private int _crouchingSound = default(int);
    [SerializeField] private int _idleSound = default(int);

    public int WalkingSpeed { get => _walkingSpeed; set => _walkingSpeed = value; }
    public int RunningSpeed { get => _runningSpeed; set => _runningSpeed = value; }
    public int CrouchingSpeed { get => _crouchingSpeed; set => _crouchingSpeed = value; }
    public int RunningSound { get => _runningSound; set => _runningSound = value; }
    public int WalkingSound { get => _walkingSound; set => _walkingSound = value; }
    public int CrouchingSound { get => _crouchingSound; set => _crouchingSound = value; }
    public int IdleSound { get => _idleSound; set => _idleSound = value; }
}
