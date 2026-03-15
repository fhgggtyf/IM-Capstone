using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayJournalManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameplayJournalDataSO _journalDataSO = null;
    [SerializeField] private UIJournalGameplay _journalUI = default;
    [SerializeField] private int _entriesUnlocked = 4;
    private GameplayJournalDataSO _actualData;
    private int _currentPageIndex = 0;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _flipToLeft;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _unlockNextEvent; 
    [SerializeField] private VoidEventChannelSO _openJournalEvent;

    private void Awake()
    {
        _inputReader.EnableJournalInput();

        _actualData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();

        _inputReader.FlipNextEvent += OnFlipNext;
        _inputReader.FlipPreviousEvent += OnFlipPrevious;
        _unlockNextEvent.OnEventRaised += UnlockNext;
        _openJournalEvent.OnEventRaised += OnJournalEntry;

        for (int i = 0; i < _entriesUnlocked; i++)
        {
            UnlockNext();
        }
        OnJournalEntry();
    }

    private void OnDisable()
    {
        _inputReader.FlipNextEvent -= OnFlipNext;
        _inputReader.FlipPreviousEvent -= OnFlipPrevious;
        _unlockNextEvent.OnEventRaised -= UnlockNext;
        _openJournalEvent.OnEventRaised -= OnJournalEntry;
    }


    void UnlockNext()
    {
        Debug.Log(_actualData.Pages);
        _actualData.Pages.Add(_journalDataSO.Pages[_currentPageIndex]);
        _currentPageIndex++;
        if (_currentPageIndex >= _entriesUnlocked)
        {
            _entriesUnlocked++;
        }
        if (_entriesUnlocked >= _journalDataSO.Pages.Count)
        {
            _entriesUnlocked = 0;
        }
    }


    private void OnJournalEntry()
    {
        Debug.Log("Journal Opened");
        _journalUI.Initialize(_actualData);
        Debug.Log("PrologueJournalManager received JournalDataSO and initialized PrologueJournalUI");

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
