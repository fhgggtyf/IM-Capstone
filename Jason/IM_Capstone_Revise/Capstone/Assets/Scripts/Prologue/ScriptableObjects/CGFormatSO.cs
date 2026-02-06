using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CG Prologue Section", menuName = "Prologue/CG")]
public class CGFormatSO : PrologueSectionSO
{
    [SerializeField] private CGDataSO _data;
    [SerializeField] public bool CanBeSkipped = true;

    [SerializeField] private SOEventChannelSO _cgInitializationEvent;

    public CGDataSO CGData => _data;

    public void InitializeCGSection()
    {
        if (_cgInitializationEvent != null)
        {
            Debug.Log("Initializing cg Section Event Raised");
            _cgInitializationEvent.RaiseEvent(_data);
        }
    }

}
                                                                                                         