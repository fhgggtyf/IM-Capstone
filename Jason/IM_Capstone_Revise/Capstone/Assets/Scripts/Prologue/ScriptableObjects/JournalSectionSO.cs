using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Journal Section", menuName = "Prologue/Journal Section")]
public class JournalSectionSO : PrologueSectionSO
{
    [SerializeField] private JournalContentSO _journalContent;

    public JournalContentSO JournalContent => _journalContent;
}
