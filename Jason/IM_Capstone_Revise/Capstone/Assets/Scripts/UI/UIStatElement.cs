using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using TMPro;

public class UIStatElement : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent _statText = default;
    [SerializeField] private IngameStatsSO _stats;
    [SerializeField] private StatTypes _statType;
    [SerializeField] private TMP_Text _value;

    private void OnEnable()
    {
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        switch (_statType)
        {
            case StatTypes.MaxHealth:
                _statText.StringReference = _stats.MaxHPLocale;
                _value.text = _stats.MaxHealth.ToString();
                break;
            case StatTypes.Armor:
                _statText.StringReference = _stats.ArmorLocale;
                _value.text = _stats.CurrentArmor.ToString();
                break;
            case StatTypes.MagicResist:
                _statText.StringReference = _stats.MRLocale;
                _value.text = _stats.CurrentMR.ToString();
                break;
            case StatTypes.Attack:
                _statText.StringReference = _stats.AtkLocale;
                _value.text = _stats.CurrentAttack.ToString();
                break;
            case StatTypes.MagicPower:
                _statText.StringReference = _stats.APLocale;
                _value.text = _stats.CurrentAP.ToString();
                break;
            case StatTypes.ArmorIgnor:
                _statText.StringReference = _stats.ArmorIgnoreLocale;
                _value.text = _stats.CurrentArmorIgnore.ToString();
                break;
            case StatTypes.MRIgnore:
                _statText.StringReference = _stats.MRIgnoreLocale;
                _value.text = _stats.CurrentMRIgnore.ToString();
                break;
            case StatTypes.Stamina:
                _statText.StringReference = _stats.StaminaLocale;
                _value.text = _stats.MaxStamina.ToString();
                break;
            case StatTypes.Tenacity:
                _statText.StringReference = _stats.TenacityLocale;
                _value.text = _stats.CurrentTenacity.ToString();
                break;
            case StatTypes.Luck:
                _statText.StringReference = _stats.LuckLocale;
                _value.text = _stats.CurrentLuck.ToString();
                break;
        }
    }
}
