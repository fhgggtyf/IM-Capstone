using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
        switch (_sections[index].sectionType)
        {
            case PrologueSectionType.Cutscene:

                break;
            case PrologueSectionType.JournalEntry:
                _journalManager.gameObject.SetActive(true);
                _videoModuleController.gameObject.SetActive(false);
                InitializeJournal();
                break;
            case PrologueSectionType.CG:
                _journalManager.gameObject.SetActive(false);
                _videoModuleController.gameObject.SetActive(true);
                InitializeCG();
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

    private void InitializeCG()
    {
        Debug.Log(_sections.Count);
        foreach (var section in _sections)
        {
            if (section is CGFormatSO)
            {
                Debug.Log("Initializing CG Section" + section);
                (section as CGFormatSO).InitializeCGSection();
            }


        }
    }
}
