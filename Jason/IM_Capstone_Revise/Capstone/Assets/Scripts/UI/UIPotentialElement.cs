using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using TMPro;

public class UIPotentialElement : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent _potentialText = default;
    [SerializeField] private IngamePotentialSO _potential;
    [SerializeField] private PotentialTypes _potentialType;
    [SerializeField] private TMP_Text _value;

    private void OnEnable()
    {
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        switch (_potentialType)
        {
            case PotentialTypes.Health:
                _potentialText.StringReference = _potential.PotentialHPLocale;
                _value.text = _potential.PotentialHealth.ToString();
                break;
            case PotentialTypes.Armor:
                _potentialText.StringReference = _potential.PotentialArmorLocale;
                _value.text = _potential.PotentialArmor.ToString();
                break;
            case PotentialTypes.MagicResist:
                _potentialText.StringReference = _potential.PotentialMRLocale;
                _value.text = _potential.PotentialMR.ToString();
                break;
            case PotentialTypes.Attack:
                _potentialText.StringReference = _potential.PotentialAtkLocale;
                _value.text = _potential.PotentialAttack.ToString();
                break;
            case PotentialTypes.MagicPower:
                _potentialText.StringReference = _potential.PotentialAPLocale;
                _value.text = _potential.PotentialAP.ToString();
                break;
            case PotentialTypes.AttackSpeed:
                _potentialText.StringReference = _potential.PotentialAttackSpeedLocale;
                _value.text = _potential.PotentialAttackSpeed.ToString();
                break;
            case PotentialTypes.Mana:
                _potentialText.StringReference = _potential.PotentialManaLocale;
                _value.text = _potential.PotentialMana.ToString();
                break;
            case PotentialTypes.Stamina:
                _potentialText.StringReference = _potential.PotentialStaminaLocale;
                _value.text = _potential.PotentialStamina.ToString();
                break;
            case PotentialTypes.Tenacity:
                _potentialText.StringReference = _potential.PotentialTenacityLocale;
                _value.text = _potential.PotentialTenacity.ToString();
                break;
            case PotentialTypes.Luck:
                _potentialText.StringReference = _potential.PotentialLuckLocale;
                _value.text = _potential.PotentialLuck.ToString();
                break;
        }
    }


}
