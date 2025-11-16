using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "EntityConfig/Enemy Stats Config")]
public class EnemyPropertiesConfigSO : ScriptableObject
{

    [SerializeField] private float _baseSpeed;


    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;
    public float closeRangeActionDistance = 1f;
    public LayerMask whatIsPlayer;

    [SerializeField] public AbilityDataSO enemyAbilityData;
    public EnemyType type;

    public float BaseSpeed { get => _baseSpeed; set => _baseSpeed = value; }
}

public enum EnemyType
{
    Health,
    Attack,
    AttackSpeed,
    Magic,
    Armor,
    MagicResist,
    Mana,
    Stamina,
    Tenacity,
    Luck,
    None
}
