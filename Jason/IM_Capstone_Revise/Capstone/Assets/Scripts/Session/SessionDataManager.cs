using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionDataManager : MonoBehaviour
{
    const int BASENUM = 50, BASELVINC = 5;

    [SerializeField] private UpgradeConfigSO _upgrades;

    [Header("Listens on")]
    [SerializeField] private VoidEventChannelSO _sessionStarted;

    [Header("Broadcasting on")]
    [SerializeField] private StringEventChannelSO _passingStats;


    private StatBlock _baseStats;
    private StatBlock _potentialStats;

    void OnEnable()
    {
        _sessionStarted.OnEventRaised += ProcessSessionData;
    }

    private void ProcessSessionData()
    {
        int totalPoints = _upgrades.PointsAvailable;
        int[] potentialLv = _upgrades.GetPotentialLevelsArray();
        int[] potential = DistributePoints(totalPoints, potentialLv.Select(x => x + 3).ToArray());

        int[] baseLv = _upgrades.GetBaseLevelsArray();
        int[] baseNum = BaseLvToNum(baseLv);

        _baseStats = StatBlock.FromArray(baseNum);
        _potentialStats = StatBlock.FromArray(potential);

        StatBlockPair basePotentialPair = new()
        {
            BaseStats = _baseStats,
            PotentialStats = _potentialStats
        };

        string serialized = JsonUtility.ToJson(basePotentialPair);

        _passingStats.RaiseEvent(serialized);
    }

    private static int[] BaseLvToNum(int[] baseLv)
    {
        int numStats = baseLv.Length;
        int[] result = new int[numStats];

        for (int i = 0; i < baseLv.Length; i++)
        {
            result[i] = BASENUM + BASELVINC * baseLv[i];
        }
        return result;
    }

    private static int[] DistributePoints(int totalPoints, int[] maxPerStat)
    {
        int numStats = maxPerStat.Length;
        int[] result = new int[numStats];

        // Step 1: Give each stat the minimum of 1
        for (int i = 0; i < numStats; i++)
        {
            result[i] = 1;
            totalPoints--;
        }

        // Step 2: Create a list of available indices to assign more points
        List<int> indices = Enumerable.Range(0, numStats).ToList();
        System.Random rng = new System.Random();

        // Step 3: Distribute remaining points
        while (totalPoints > 0 && indices.Count > 0)
        {
            int idx = indices[rng.Next(indices.Count)];

            if (result[idx] < maxPerStat[idx])
            {
                result[idx]++;
                totalPoints--;
            }

            // Remove from list if it hit its max
            if (result[idx] >= maxPerStat[idx])
            {
                indices.Remove(idx);
            }
        }

        return result;
    }

    private void OnDisable()
    {
        _sessionStarted.OnEventRaised -= ProcessSessionData;
    }
}

[Serializable]
public class StatBlockPair
{
    public StatBlock BaseStats;
    public StatBlock PotentialStats;
}
