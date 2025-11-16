using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIManaBarManager : UIBarManager
{

    protected override void UpdateBar()
    {
        _percentage = (float)_protagonistStats.CurrentMana / _protagonistStats.MaxMana;
        StartCoroutine(SmoothBar());

        _text.SetText(Mathf.FloorToInt((float)_protagonistStats.CurrentMana).ToString());
    }
}
