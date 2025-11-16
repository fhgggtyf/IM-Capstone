using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// An instance of the Stats of a character, be it the player or an NPC.
/// The initial values are usually contained in another SO of type statsConfigSO.
/// </summary>
[CreateAssetMenu(fileName = "PlayersIngamePotential", menuName = "EntityConfig/Player's Potential")]
public class IngamePotentialSO : ScriptableObject
{
    [Tooltip("Stats")]
    [SerializeField][ReadOnly] private int _potentialHealth;
    [SerializeField][ReadOnly] private int _potentialArmor;
    [SerializeField][ReadOnly] private int _potentialMR;
    [SerializeField][ReadOnly] private int _potentialAttack;
    [SerializeField][ReadOnly] private int _potentialAP;
    [SerializeField][ReadOnly] private float _potentialAttackSpeed;
    [SerializeField][ReadOnly] private int _potentialMana;
    [SerializeField][ReadOnly] private int _potentialTenacity;
    [SerializeField][ReadOnly] private float _potentialStamina;
    [SerializeField][ReadOnly] private int _potentialLuck;

    [Tooltip("Localization")]
    [SerializeField] public LocalizedString PotentialHPLocale;
    [SerializeField] public LocalizedString PotentialArmorLocale;
    [SerializeField] public LocalizedString PotentialMRLocale;
    [SerializeField] public LocalizedString PotentialAtkLocale;
    [SerializeField] public LocalizedString PotentialAPLocale;
    [SerializeField] public LocalizedString PotentialTenacityLocale;
    [SerializeField] public LocalizedString PotentialStaminaLocale;
    [SerializeField] public LocalizedString PotentialLuckLocale;
    [SerializeField] public LocalizedString PotentialAttackSpeedLocale;
    [SerializeField] public LocalizedString PotentialManaLocale;


    public int PotentialHealth { get => _potentialHealth; }
    public int PotentialArmor { get => _potentialArmor; }
    public int PotentialMR { get => _potentialMR; }
    public int PotentialAttack { get => _potentialAttack; }
    public int PotentialAP { get => _potentialAP; }
    public float PotentialAttackSpeed { get => _potentialAttackSpeed; }
    public int PotentialMana { get => _potentialMana; }
    public int PotentialTenacity { get => _potentialTenacity; }
    public float PotentialStamina { get => _potentialStamina; }
    public int PotentialLuck { get => _potentialLuck; }
}
