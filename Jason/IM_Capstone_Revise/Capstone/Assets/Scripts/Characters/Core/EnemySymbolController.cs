using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbolController : CoreComponent
{
    public GameObject AlertSymbol;
    public GameObject SusSymbol;

    public void ToggleAlertSymbol(bool toggle)
    {
        AlertSymbol.SetActive(toggle);
    }

    public void ToggleSusSymbol(bool toggle)
    {
        SusSymbol.SetActive(toggle);
    }
}
