using UnityEngine;

[CreateAssetMenu(fileName = "StatsConfig", menuName = "EntityConfig/Enemy Stats Config")]
public class EnemyPropertiesConfigSO : ScriptableObject
{
    [SerializeField] private int _soundThreshold = default(int);

    public LayerMask whatIsPlayer;

    public int SoundThreshold { get => _soundThreshold; set => _soundThreshold = value; }

}


