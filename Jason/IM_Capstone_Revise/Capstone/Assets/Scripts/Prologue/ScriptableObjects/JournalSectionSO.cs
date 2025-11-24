using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Journal Section", menuName = "Prologue/Journal Section")]
public class JournalSectionSO : PrologueSectionSO
{
    [SerializeField] private JournalDataSO _journalContent;

    [Header("Broadcasting on")]
    [SerializeField] private SOEventChannelSO _journalInitializationEvent;
    [SerializeField] private IntEventChannelSO _journalUpdatePageAccessEvent;

    public JournalDataSO JournalContent => _journalContent;

    public void InitializeJournalSection() 
    {
        if (_journalInitializationEvent != null)
        {
            Debug.Log("Initializing Journal Section Event Raised");
            _journalInitializationEvent.RaiseEvent(_journalContent);
        }
    }

    public override void Play()
    {
        base.Play();
        if (_journalUpdatePageAccessEvent != null)
            _journalUpdatePageAccessEvent.RaiseEvent(_journalContent.Pages.Count/2);
    }
}
