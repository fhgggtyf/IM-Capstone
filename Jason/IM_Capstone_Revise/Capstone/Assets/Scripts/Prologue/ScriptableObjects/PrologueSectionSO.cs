using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSectionSO : ScriptableObject
{
    public PrologueSectionType sectionType;
    public AudioCueSO audioCue;

    public virtual void Play()
    {
        Debug.Log("Playing Prologue Section: " + sectionType);
    }
}

public enum PrologueSectionType
{
    Cutscene,
    JournalEntry,
    CG
}
