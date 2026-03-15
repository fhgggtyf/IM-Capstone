using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueJournalManager : MonoBehaviour
{

    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private JournalDataSO _journalDataSO = null;
    [SerializeField] private PrologueJournalUI _journalUI = default;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _flipToLeft;

    [Header("Listening to")]
    [SerializeField] private SOEventChannelSO _openJournalEvent;

    private void OnEnable()
    {
        _inputReader.EnableJournalInput();

        _inputReader.FlipNextEvent += OnFlipNext;
        _inputReader.FlipPreviousEvent += OnFlipPrevious;
        _openJournalEvent.OnEventRaised += OnJournalEntry; 
    }
    
    private void OnDisable()
    {
        _inputReader.FlipNextEvent -= OnFlipNext;
        _inputReader.FlipPreviousEvent -= OnFlipPrevious;
        _openJournalEvent.OnEventRaised -= OnJournalEntry;
    }

    private void OnJournalEntry(ScriptableObject journalContentData)
    {
        if ( journalContentData is JournalDataSO)
        {
            _journalDataSO = journalContentData as JournalDataSO;
            _journalUI.Initialize(_journalDataSO);
            Debug.Log("PrologueJournalManager received JournalDataSO and initialized PrologueJournalUI");
        }
        else 
        {         
            Debug.LogError("PrologueJournalManager received invalid JournalDataSO");
        }
    }

    private void OnFlipNext()
    {
        Debug.Log("Flip Next");
        _flipToLeft.RaiseEvent(true);
    }
    private void OnFlipPrevious()
    {
        _flipToLeft.RaiseEvent(false);
    }
}
