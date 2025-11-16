using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIStaminaBarManager : UIBarManager
{
    protected override void UpdateBar()
    {
        _percentage = (float)_protagonistStats.CurrentStamina / _protagonistStats.MaxStamina;
        StartCoroutine(SmoothBar());

        _text.SetText(Mathf.FloorToInt(_protagonistStats.CurrentStamina).ToString());
    }
}
