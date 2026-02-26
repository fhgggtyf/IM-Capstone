using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class PrologueManager : MonoBehaviour
{

    [SerializeField] private GameStateSO _gameState = default;
    [SerializeField] private List<PrologueSectionSO> _sections;
    [SerializeField] private JournalManager _journalManager;
    [SerializeField] private VideoModuleController _videoModuleController;
    private int _index = 0;


    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _prologueEndedEvent;
    [SerializeField] private VoidEventChannelSO _initializeJournalEvent;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _journalSectionEndedEvent;
    [SerializeField] private VoidEventChannelSO _cgSectionEndedEvent;

    private void Start()
    {
        StartPrologue();
    }

    private void OnEnable()
    {
        _journalSectionEndedEvent.OnEventRaised += InitializeNextJournal;
        _journalSectionEndedEvent.OnEventRaised += NextSection;
        _cgSectionEndedEvent.OnEventRaised += InitializeNextCG;
        _cgSectionEndedEvent.OnEventRaised += NextSection;
    }

    private void OnDisable()
    {
        _journalSectionEndedEvent.OnEventRaised -= InitializeNextJournal;
        _journalSectionEndedEvent.OnEventRaised -= NextSection;
        _cgSectionEndedEvent.OnEventRaised -= InitializeNextCG;
        _cgSectionEndedEvent.OnEventRaised -= NextSection;
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
        switch (_sections[index].sectionType)
        {
            case PrologueSectionType.Cutscene:

                break;
            case PrologueSectionType.JournalEntry:
                _journalManager.gameObject.SetActive(true);
                _videoModuleController.gameObject.SetActive(false);
                InitializeJournal(_sections[index]);
                break;
            case PrologueSectionType.CG:
                _journalManager.gameObject.SetActive(false);
                _videoModuleController.gameObject.SetActive(true);
                InitializeCG(_sections[index]);
                break;
        }
        _sections[index].Play();
    }

    void EndPrologue()
    {
        _prologueEndedEvent.RaiseEvent();
    }


    private void StartPrologue()
    {
        _gameState.UpdateGameState(GameState.Prologue);

        PlaySection(0);
    }

    private void InitializeJournal(PrologueSectionSO section)
    {
        Debug.Log(_sections.Count);

        if (section is JournalSectionSO)
        {
            Debug.Log("Initializing Journal Section" + section);
            (section as JournalSectionSO).InitializeJournalSection();
        }

    }

    private void InitializeCG(PrologueSectionSO section)
    {
        Debug.Log(_sections.Count);

        if (section is CGFormatSO)
        {
            Debug.Log("Initializing CG Section" + section);
            (section as CGFormatSO).InitializeCGSection();
        }

    }

    private void InitializeNextCG()
    {
        int next = Enumerable
            .Range(_index + 1, _sections.Count - (_index + 1))
            .FirstOrDefault(j => _sections[j] is CGFormatSO);

        if (next <= _index) next = -1;

        if (next != -1)
        {
            Debug.Log("Initializing Next CG Section" + _sections[next]);
            InitializeCG(_sections[next]);
        }

    }

    private void InitializeNextJournal()
    {
        int next = Enumerable
            .Range(_index + 1, _sections.Count - (_index + 1))
            .FirstOrDefault(j => _sections[j] is JournalSectionSO);

        if (next <= _index) next = -1;

        if (next != -1)
        {
            Debug.Log("Initializing Next Journal Section" + _sections[next]);
            InitializeJournal(_sections[next]);
        }
    }
}
