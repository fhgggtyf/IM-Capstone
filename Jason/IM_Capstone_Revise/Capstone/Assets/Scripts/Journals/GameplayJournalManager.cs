using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayJournalManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameplayJournalDataSO _journalDataSO = null;
    [SerializeField] private UIJournalGameplay _journalUI = default;
    [SerializeField] private int _entriesUnlocked = 5;
    private GameplayJournalDataSO _initialData;
    private GameplayJournalDataSO _addedData;
    private bool _initialized = false;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _flipToLeft;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _unlockNextEvent; 
    [SerializeField] private VoidEventChannelSO _openJournalEvent;

    private void Awake()
    {
        _inputReader.EnableJournalInput();

        _initialData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();
        _addedData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();

        _inputReader.FlipNextEvent += OnFlipNext;
        _inputReader.FlipPreviousEvent += OnFlipPrevious;
        _unlockNextEvent.OnEventRaised += UnlockNext;

        for (int i = 0; i < _entriesUnlocked; i++)
        {
            _initialData.Pages.Add(_journalDataSO.Pages[i]);
        }
        OnJournalEntry();
    }

    private void OnDestroy()
    {
        _inputReader.FlipNextEvent -= OnFlipNext;
        _inputReader.FlipPreviousEvent -= OnFlipPrevious;
        _unlockNextEvent.OnEventRaised -= UnlockNext;
        _openJournalEvent.OnEventRaised -= OnJournalEntry;
    }


    void UnlockNext()
    {
        _openJournalEvent.OnEventRaised += OnJournalEntry;
        Debug.Log(_initialData.Pages);
        _addedData.Pages.Add(_journalDataSO.Pages[_entriesUnlocked]);
        _entriesUnlocked++;
        if (_entriesUnlocked >= _journalDataSO.Pages.Count)
        {
            _entriesUnlocked = 0;
        }
    }

    private void OnJournalEntry()
    {
        if (!_initialized)
        {
            _initialized = true;
            Debug.Log("Initializing journal UI for the first time.");
            _journalUI.Initialize(_initialData);
        }
        else
        {
            _openJournalEvent.OnEventRaised -= OnJournalEntry;
            Debug.Log("Updating journal UI with new data.");
            _journalUI.Initialize(_addedData);
            _addedData.Pages.Clear();
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
