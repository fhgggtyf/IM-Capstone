using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CG Prologue Section", menuName = "Prologue/CG")]
public class CGFormatSO : PrologueSectionSO
{
    [SerializeField] private CGDataSO _data;
    [SerializeField] public bool CanBeSkipped = true;

    public CGDataSO CGData => _data;

}
                                                                                                         