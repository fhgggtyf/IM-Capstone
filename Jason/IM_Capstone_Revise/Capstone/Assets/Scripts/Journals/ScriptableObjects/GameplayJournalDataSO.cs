using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "new Journal Section", menuName = "Journals/Gameplay Journal Data")]
public class GameplayJournalDataSO : ScriptableObject
{
    [SerializeField] private List<GameplayJournalContentSO> _pages = new();
    [SerializeField] private VoidEventChannelSO _endOfJournalSectionEvent = default;
    public VoidEventChannelSO EndOfJournaSectionEvent => _endOfJournalSectionEvent;
    public List<GameplayJournalContentSO> Pages => _pages;

    public void FinishJournalSection()
    {
        if (EndOfJournaSectionEvent != null)
            EndOfJournaSectionEvent.RaiseEvent();
    }
}
