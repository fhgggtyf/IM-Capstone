using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new Journal Section", menuName = "Journals/Journal Data")]
public class JournalDataSO : ScriptableObject
{
    [SerializeField] private List<PrologueJournalContentSO> _pages = default;
    [SerializeField] private VoidEventChannelSO _endOfJournalSectionEvent = default;
    public VoidEventChannelSO EndOfJournaSectionEvent => _endOfJournalSectionEvent;
    public List<PrologueJournalContentSO> Pages => _pages;

    public void FinishJournalSection ()
    {
        if (EndOfJournaSectionEvent != null)
            EndOfJournaSectionEvent.RaiseEvent();
    }
}
