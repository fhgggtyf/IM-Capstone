using UnityEngine;

public class DamageData
{
    public float Amount { get; private set; }
    public int ArmorIgnore { get; private set; }
    public int MrIgnore { get; private set; }
    public AbilityDataSO AbilityParam;
    public EnemyType EnemyType;

    public GameObject Source { get; private set; }

    public DamageData(float amount, int armorIgnore, int mrIgnore, AbilityDataSO abilityData, EnemyType enemyType, GameObject source)
    {
        Amount = amount;
        Source = source;
        ArmorIgnore = armorIgnore;
        MrIgnore = mrIgnore;
        AbilityParam = abilityData;
        EnemyType = enemyType;
    }

    public void SetAmount(float amount)
    {
        Amount = amount;
    }
}

public enum DamageType
{
    Physical,
    Magical,
    True
}
