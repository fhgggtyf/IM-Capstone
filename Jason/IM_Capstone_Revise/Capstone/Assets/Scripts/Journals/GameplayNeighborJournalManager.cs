using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayNeighborJournalManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameplayJournalDataSO _journalDataSO = null;
    [SerializeField] private UIJournalGameplay _journalUI = default;
    [SerializeField] private int _entriesUnlocked = 2;
    private GameplayJournalDataSO _initialData;
    private GameplayJournalDataSO _addedData;
    private bool _initialized = false;

    [Header("Broadcasting on")]
    [SerializeField] private BoolEventChannelSO _flipToLeft;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _openJournalEvent;

    private void Awake()
    {
        //_inputReader.EnableJournalInput();

        _initialData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();
        _addedData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();

        for (int i = 0; i < _entriesUnlocked; i++)
        {
            _initialData.Pages.Add(_journalDataSO.Pages[i]);
        }
        OnJournalEntry();

        _openJournalEvent.OnEventRaised += OnJournalEntry;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _openJournalEvent.OnEventRaised -= OnJournalEntry;
    }

    private void OnJournalEntry()
    {
        if (!_initialized)
        {
            _initialized = true;
            Debug.Log("Initializing journal UI for the first time.");
            _journalUI.Initialize(_initialData);
        }
    }
}
