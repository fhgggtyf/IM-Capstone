using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSectionSO : ScriptableObject
{
    public PrologueSectionType sectionType;
}

public enum PrologueSectionType
{
    Cutscene,
    JournalEntry,
    CG
}
