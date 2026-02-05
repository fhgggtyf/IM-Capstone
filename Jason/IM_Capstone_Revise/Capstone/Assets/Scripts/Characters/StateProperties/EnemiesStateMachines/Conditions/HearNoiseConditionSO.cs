using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// Condition that evaluates whether the enemy has heard the player via sound. It
/// calculates the distance between the enemy and the player and compares that
/// distance against the sum of the player's noise radius and the enemy's own
/// sound detection threshold. If the circles defined by these radii overlap,
/// the condition becomes true and stores the player's current position on the
/// NonPlayerCharacter for investigation.
/// </summary>
[CreateAssetMenu(fileName = "HearNoiseCondition", menuName = "State Machines/Conditions/Enemies/Hear Noise")]
public class HearNoiseConditionSO : StateConditionSO<HearNoiseCondition>
{
}

public class HearNoiseCondition : Condition
{
    private NonPlayerCharacter _npc;
    private NonPlayerStatsManager _enemyStats;
    private Player _player;
    private PlayerStatsManager _playerStats;

    public override void Awake(StateMachine stateMachine)
    {
        _npc = stateMachine.GetComponent<NonPlayerCharacter>();
        _enemyStats = stateMachine.GetComponent<NonPlayerStatsManager>();
        // Attempt to find the player in the scene. It is assumed there is only one
        // Player instance in the game. If there are multiple players this logic
        // should be adjusted accordingly.
        _player = GameObject.FindObjectOfType<Player>();
        if (_player != null)
        {
            _playerStats = _player.GetComponent<PlayerStatsManager>();
        }
    }

    protected override bool Statement()
    {
        // If we already heard the player, avoid repeated triggers until reset in investigate state
        if (_npc.hasHeardPlayer)
            return false;

        if (_player == null || _playerStats == null || _enemyStats == null)
        {
            _player = GameObject.FindObjectOfType<Player>();
            if (_player != null)
            {
                _playerStats = _player.GetComponent<PlayerStatsManager>();
            }
            return false;
        }

        // Player's noise radius (how far the noise travels)
        float playerNoiseRadius = _playerStats.CurrentNoise;
        if (playerNoiseRadius <= 0f)
            return false;

        // Enemy's sound detection radius (threshold)
        float enemyDetectRadius = _enemyStats.GetSoundThreshold();
        if (enemyDetectRadius <= 0f)
            return false;

        // Distance between enemy and player
        float distance = Vector2.Distance(_npc.transform.position, _player.transform.position);

        // If the player's noise circle overlaps the enemy's detection circle, we heard something
        bool heard = distance <= (playerNoiseRadius + enemyDetectRadius);
        if (heard)
        {
            _npc.hasHeardPlayer = true;
            _npc.lastHeardPosition = _player.transform.position;
        }

        return heard;
    }

    public override void OnStateExit()
    {
        // Do nothing on exit. Flags are reset by the investigation state.
    }
}