using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{

    [SerializeField] private GameStateSO _gameState = default;
    [SerializeField] private List<PrologueSectionSO> _sections;
    private int _index = 0;


    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _prologueEndedEvent;
    [SerializeField] private VoidEventChannelSO _initializeJournalEvent;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _journalSectionEndedEvent;

    private void Start()
    {
        StartPrologue();
    }

    private void OnEnable()
    {
        _journalSectionEndedEvent.OnEventRaised += NextSection;
    }

    private void OnDisable()
    {
        _journalSectionEndedEvent.OnEventRaised -= NextSection;
    }

    public void NextSection() => PlaySection(_index + 1);

    void PlaySection(int index)
    {
        if (index >= _sections.Count)
        {
            EndPrologue();
            return;
        }

        _index = index;
        _sections[index].Play();
    }

    void EndPrologue()
    {
        _prologueEndedEvent.RaiseEvent();
    }


    private void StartPrologue()
    {
        _gameState.UpdateGameState(GameState.Prologue);

        InitializeJournal();

        PlaySection(0);
    }

    private void InitializeJournal()
    {
        Debug.Log(_sections.Count);
        foreach (var section in _sections)
        {
            if (section is JournalSectionSO)
            {
                Debug.Log("Initializing Journal Section" + section);
                (section as JournalSectionSO).InitializeJournalSection();
            }
        }
    }

}
