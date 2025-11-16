using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionStatsManager : MonoBehaviour
{
    [SerializeField] private StatsConfigSO _playerStats;

    [Header("Listening on")]
    [SerializeField] private TransformEventChannelSO _playerInstantiated;
    [SerializeField] private StringEventChannelSO _statsPairChannel;

    [Header("Broadcasting on")]
    [SerializeField] private SOEventChannelSO _stats;
    [SerializeField] private VoidEventChannelSO _onSessionStarted;


    private StatBlock _baseStats;
    private StatBlock _potentialStats;

    private void OnEnable()
    {

        _onSessionStarted.RaiseEvent();

        _statsPairChannel.OnEventRaised += ParseStats;
        _playerInstantiated.OnEventRaised += SendStats;

        //on death trigger recalculation of base and potential (unimplemented)
    }

    private void ParseStats(string serialized)
    {
        StatBlockPair deserialized = JsonUtility.FromJson<StatBlockPair>(serialized);
        _baseStats = deserialized.BaseStats;
        _potentialStats = deserialized.PotentialStats;

        _playerStats.InitializeBaseStats(_baseStats);


    }

    private void SendStats(Transform playerTransform)
    {
        _stats.RaiseEvent(_playerStats);
    }

    private void OnDisable()
    {
        _statsPairChannel.OnEventRaised -= ParseStats;
        _playerInstantiated.OnEventRaised -= SendStats;
    }
}
