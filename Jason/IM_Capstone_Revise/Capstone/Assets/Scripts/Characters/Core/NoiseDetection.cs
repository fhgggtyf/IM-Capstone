using UnityEngine;

/// <summary>
/// Core component responsible for detecting player-generated noise and
/// maintaining information about the last noise event. It performs the
/// detection logic each frame in <see cref="LogicUpdate"/> and exposes
/// whether a new noise has been detected since the last reset as well as
/// the world position where the noise originated. This separates the
/// sensing of noise from state machine conditions so that detection can
/// be reused by multiple systems and does not couple sensing with
/// state transitions.
/// </summary>
public class NoiseDetection : CoreComponent
{
    private NonPlayerCharacter _npc;
    private NonPlayerStatsManager _enemyStats;
    private Player _player;
    private PlayerStatsManager _playerStats;

    /// <summary>
    /// Indicates whether the enemy has heard a noise and should react. This
    /// flag remains true until cleared via <see cref="ClearDetection"/> so
    /// that continuous noises do not cause repeated triggers.
    /// </summary>
    public bool Detected { get; private set; }

    /// <summary>
    /// Indicates that a new noise has been detected since the previous
    /// update cycle. This flag is set when a noise is first detected and
    /// reset on subsequent frames while the noise persists. Consumers
    /// should rely on this flag to trigger one‑shot reactions.
    /// </summary>
    public bool NewDetection { get; private set; }

    /// <summary>
    /// The world position where the most recent noise originated. This is
    /// updated whenever the player’s noise is within the detection range.
    /// </summary>
    public Vector2 LastHeardPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // Attempt to find required components on the root of the entity.  The Core
        // sits on a child GameObject so we query the root for character and stats.
        if (core != null && core.Root != null)
        {
            _npc = core.Root.GetComponent<NonPlayerCharacter>();
            _enemyStats = core.Root.GetComponent<NonPlayerStatsManager>();
        }
    }

    /// <summary>
    /// Performs noise detection each frame.  When the player is within
    /// combined noise and detection radii, the component records the
    /// position of the noise and sets the detection flags accordingly.  The
    /// flags remain set until <see cref="ClearDetection"/> is called.
    /// </summary>
    public override void LogicUpdate()
    {
        // Look up the player lazily on demand.  This avoids repeated
        // GameObject.Find calls once the reference is cached.
        if (_player == null)
        {
            _player = GameObject.FindObjectOfType<Player>();
            if (_player != null)
            {
                _playerStats = _player.GetComponent<PlayerStatsManager>();
            }
        }
        // If any required component is missing, do not attempt detection.
        if (_npc == null || _enemyStats == null || _player == null || _playerStats == null)
            return;

        // Retrieve noise radii from the player and detection threshold from the enemy.
        float playerNoiseRadius = _playerStats.CurrentNoise;
        float enemyDetectRadius = _enemyStats.GetSoundThreshold();
        if (playerNoiseRadius <= 0f || enemyDetectRadius <= 0f)
            return;

        // Calculate distance between enemy and player in 2D space.
        float distance = Vector2.Distance(_npc.transform.position, _player.transform.position);
        bool heard = distance <= (playerNoiseRadius + enemyDetectRadius);

        if (heard)
        {
            // Update the last known position of the noise stimulus.
            LastHeardPosition = _player.transform.position;
            if (!Detected)
            {
                // This is the first time a noise has been detected in this cycle.
                Detected = true;
                NewDetection = true;
            }
            else
            {
                // Noise persists but this is not a new detection.
                NewDetection = false;
            }
        }
    }

    /// <summary>
    /// Clears the detection flags and last heard position.  Call this when
    /// the investigation has completed or the enemy should forget about the
    /// noise stimulus.
    /// </summary>
    public void ClearDetection()
    {
        Detected = false;
        NewDetection = false;
        LastHeardPosition = Vector2.zero;
    }
}