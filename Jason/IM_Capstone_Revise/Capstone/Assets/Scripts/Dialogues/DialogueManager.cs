using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<ActorSO> _actorsList = default;
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameStateSO _gameState = default;
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
    [SerializeField] private InteractableBackgroundController _interactableBackgroundController = default;

    [Header("Listening on")]
    [SerializeField] private DialogueDataChannelSO _startDialogue = default;
    [SerializeField] private DialogueDataChannelSO _insertDialogue = default;          // NEW: insert event
    [SerializeField] private DialogueChoiceChannelSO _makeDialogueChoiceEvent = default;

    [Header("Broadcasting on")]
    [SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
    [SerializeField] private DialogueChoicesChannelSO _showChoicesUIEvent = default;
    [SerializeField] private IntEventChannelSO _endDialogueWithTypeEvent = default;
    [SerializeField] private VoidEventChannelSO _continueWithStep = default;
    [SerializeField] private VoidEventChannelSO _playIncompleteDialogue = default;
    [SerializeField] private VoidEventChannelSO _makeWinningChoice = default;
    [SerializeField] private VoidEventChannelSO _makeLosingChoice = default;
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;

    private int _counterDialogue;
    private int _counterLine;
    private bool _reachedEndOfDialogue { get => _counterDialogue >= _currentDialogue.Lines.Count; }
    private bool _reachedEndOfLine { get => _counterLine >= _currentDialogue.Lines[_counterDialogue].TextList.Count; }
    private DialogueDataSO _currentDialogue = default;
    private AudioCueKey _currentLineAudioCue;

    // Stack to store interrupted dialogue contexts
    private struct DialogueContext
    {
        public DialogueDataSO Dialogue;
        public int CounterDialogue;
        public int CounterLine;
    }
    private Stack<DialogueContext> _dialogueStack = new Stack<DialogueContext>();

    private void Start()
    {
        _startDialogue.OnEventRaised += DisplayDialogueData;
        if (_insertDialogue != null)
            _insertDialogue.OnEventRaised += InsertDialogue;          // NEW subscription
        _currentLineAudioCue = AudioCueKey.Invalid;
    }

    private void OnDestroy()
    {
        _startDialogue.OnEventRaised -= DisplayDialogueData;
        if (_insertDialogue != null)
            _insertDialogue.OnEventRaised -= InsertDialogue;          // NEW unsubscription
        _inputReader.AdvanceDialogueEvent -= OnAdvance;
        _makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;
    }

    /// <summary>
    /// Starts a fresh dialogue, discarding any pending inserted dialogues.
    /// </summary>
    public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
    {
        // Clear any pending insertion contexts – this is a brand new conversation
        _dialogueStack.Clear();
        if (_currentLineAudioCue != AudioCueKey.Invalid)
            _sfxEventChannel.RaiseStopEvent(_currentLineAudioCue);

        if (_gameState.CurrentGameState != GameState.Cutscene)
            _gameState.UpdateGameState(GameState.Dialogue);

        _counterDialogue = 0;
        _counterLine = 0;
        _inputReader.EnableDialogueInput();
        _inputReader.AdvanceDialogueEvent += OnAdvance;
        _currentDialogue = dialogueDataSO;

        if (_currentDialogue.RelatedInteractiveItem != null && _currentDialogue.InteractableBG != null)
        {
            Debug.Log("Init " + _currentDialogue.RelatedInteractiveItem + " " + _currentDialogue.InteractableBG);
            ShowInteractableBackground(_currentDialogue.RelatedInteractiveItem, _currentDialogue.InteractableBG);
        }

        if (_currentDialogue.Lines != null)
        {
            ShowCurrentLine();
        }
        else
        {
            Debug.LogError("Check Dialogue");
        }
    }

    /// <summary>
    /// Inserts a dialogue into the current conversation. The currently playing line will be shown
    /// after the inserted dialogue finishes.
    /// </summary>
    public void InsertDialogue(DialogueDataSO dialogueToInsert)
    {
        if (_currentDialogue == null)
        {
            // No active dialogue – just start it normally
            DisplayDialogueData(dialogueToInsert);
            return;
        }

        // Save current state onto stack
        _dialogueStack.Push(new DialogueContext
        {
            Dialogue = _currentDialogue,
            CounterDialogue = _counterDialogue,
            CounterLine = _counterLine
        });

        // Stop current line audio
        if (_currentLineAudioCue != AudioCueKey.Invalid)
            _sfxEventChannel.RaiseStopEvent(_currentLineAudioCue);

        if (_currentDialogue.RelatedInteractiveItem != null && _currentDialogue.InteractableBG != null)
            DeactivateBackground();

        // Switch to inserted dialogue and start from beginning
        _currentDialogue = dialogueToInsert;
        _counterDialogue = 0;
        _counterLine = 0;

        if (_currentDialogue.RelatedInteractiveItem != null && _currentDialogue.InteractableBG != null)
        {
            Debug.Log("Init " + _currentDialogue.RelatedInteractiveItem + " " + _currentDialogue.InteractableBG);
            _dialogueStack.Clear();
            ShowInteractableBackground(_currentDialogue.RelatedInteractiveItem, _currentDialogue.InteractableBG);
        }

        // Show first line of inserted dialogue (no game state change, input already enabled)
        ShowCurrentLine();
    }

    /// <summary>
    /// Displays the current line (based on _counterDialogue and _counterLine) in the UI,
    /// including actor, text, audio and close‑up image.
    /// </summary>
    private void ShowCurrentLine()
    {
        if (_currentDialogue == null || _currentDialogue.Lines == null || _reachedEndOfDialogue)
            return;

        ActorSO currentActor = _actorsList.Find(o => o.ActorId == _currentDialogue.Lines[_counterDialogue].Actor);
        AudioCueSO audioCue = (_currentDialogue.Lines[_counterDialogue].LineAudio != null && _currentDialogue.Lines[_counterDialogue].LineAudio.Count > _counterLine)
            ? _currentDialogue.Lines[_counterDialogue].LineAudio[_counterLine]
            : null;

        // Stop any previous audio before playing new one
        if (_currentLineAudioCue != AudioCueKey.Invalid)
            _sfxEventChannel.RaiseStopEvent(_currentLineAudioCue);

        PresentDialogueLine(
            _currentDialogue.Lines[_counterDialogue].TextList[_counterLine],
            currentActor,
            audioCue,
            _currentDialogue.Lines[_counterDialogue].CloseUpImage
        );
    }

    private void ShowInteractableBackground(List<InteractiveItemDataSO> interactable, Sprite bg)
    {
        _interactableBackgroundController.InteractiveItems = interactable;
        _interactableBackgroundController.InitializePanel(interactable, bg);
    }
    private void DeactivateBackground()
    {
        Debug.Log("ALLLLLL CLEAARRRRRR");
        _interactableBackgroundController.ClearItems();
    }


    void DebugList(string tag)
    {
        Debug.Log(
            $"{tag} | obj='{name}' id={GetInstanceID()} scene={gameObject.scene.name} " +
            $"active={gameObject.activeInHierarchy} listCount={(_actorsList == null ? -1 : _actorsList.Count)} " +
            $"path={GetPath(transform)}",
            this
        );
    }

    static string GetPath(Transform t)
    {
        var sb = new StringBuilder(t.name);
        while (t.parent != null) { t = t.parent; sb.Insert(0, t.name + "/"); }
        return sb.ToString();
    }

    /// <summary>
    /// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
    /// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
    /// </summary>
    public void PresentDialogueLine(LocalizedString dialogueLine, ActorSO actor, AudioCueSO audioCue, Sprite closeUp = null)
    {
        _openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
        if (audioCue != null)
        {
            Debug.Log("Audio: " + audioCue + " audioconfig: " + _audioConfiguration);
            _currentLineAudioCue = _sfxEventChannel.RaisePlayEvent(audioCue, _audioConfiguration);
        }
        else
        {
            _currentLineAudioCue = AudioCueKey.Invalid;
        }

        if (closeUp != null)
        {
            _interactableBackgroundController.InitializePanel(null, closeUp);
        }
    }

    private void OnAdvance()
    {
        _counterLine++;
        if (!_reachedEndOfLine)
        {
            ShowCurrentLine();
        }
        else if (_currentDialogue.Lines[_counterDialogue].Choices != null
                 && _currentDialogue.Lines[_counterDialogue].Choices.Count > 0)
        {
            DisplayChoices(_currentDialogue.Lines[_counterDialogue].Choices);
        }
        else
        {
            _counterDialogue++;
            if (!_reachedEndOfDialogue)
            {
                _counterLine = 0;
                ShowCurrentLine();
            }
            else
            {
                DialogueEndedAndCloseDialogueUI();
            }
        }
    }

    private void DisplayChoices(List<Choice> choices)
    {
        _inputReader.AdvanceDialogueEvent -= OnAdvance;
        _makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
        _showChoicesUIEvent.RaiseEvent(choices);
    }

    private void MakeDialogueChoice(Choice choice)
    {
        _makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;

        Debug.Log("Choice made: " + choice.Response + ", Action Type: " + choice.ActionType);

        switch (choice.ActionType)
        {
            case ChoiceActionType.ContinueWithStep:
                if (_continueWithStep != null)
                    _continueWithStep.RaiseEvent();
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                break;

            case ChoiceActionType.WinningChoice:
                if (_makeWinningChoice != null)
                    _makeWinningChoice.RaiseEvent();
                break;

            case ChoiceActionType.LosingChoice:
                if (_makeLosingChoice != null)
                    _makeLosingChoice.RaiseEvent();
                break;

            case ChoiceActionType.DoNothing:
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                else
                    DialogueEndedAndCloseDialogueUI();
                break;

            case ChoiceActionType.IncompleteStep:
                if (_playIncompleteDialogue != null)
                {
                    _playIncompleteDialogue.RaiseEvent();
                    Debug.Log("Raising incomplete dialogue event");
                }
                if (choice.NextDialogue != null)
                {
                    Debug.Log("Displaying incomplete dialogue");
                    DisplayDialogueData(choice.NextDialogue);
                }
                break;
        }
    }

    public void CutsceneDialogueEnded()
    {
        if (_endDialogueWithTypeEvent != null)
            _endDialogueWithTypeEvent.RaiseEvent((int)DialogueType.DefaultDialogue);
    }

    private void RestoreCurrentDialogueBackground()
    {
        // Reapply the background and interactive items for the current dialogue
        if (_currentDialogue.RelatedInteractiveItem != null && _currentDialogue.InteractableBG != null)
        {
            Debug.Log("restoring "+ _currentDialogue +" " + _currentDialogue.InteractableBG);
            ShowInteractableBackground(_currentDialogue.RelatedInteractiveItem, _currentDialogue.InteractableBG);
        }
        // Optional: If you have a default "empty" background, you could clear it here.
        // Otherwise, the previous background (e.g., from the inserted dialogue) will remain – 
        // but we want to override it, so the condition above must be true for the original dialogue.
        // If the original dialogue had no background, you may want to explicitly clear the panel.
        // For now, we mirror the original logic: only set if both are non‑null.
    }

    private void DialogueEndedAndCloseDialogueUI()
    {
        // Stop current line audio
        if (_currentLineAudioCue != AudioCueKey.Invalid)
            _sfxEventChannel.RaiseStopEvent(_currentLineAudioCue);

        // Check if there is a pending inserted dialogue context
        if (_dialogueStack.Count > 0)
        {
            // Restore previous dialogue state
            DialogueContext context = _dialogueStack.Pop();
            _currentDialogue = context.Dialogue;
            _counterDialogue = context.CounterDialogue;
            _counterLine = context.CounterLine;

            // ✅ Reapply the original dialogue's background and interactive items
            RestoreCurrentDialogueBackground();

            // Resume the interrupted line
            ShowCurrentLine();
            return; // Do not end the overall dialogue session
        }

        DeactivateBackground();

        // No pending contexts → fully end the dialogue (original code unchanged)
        _currentDialogue.FinishDialogue();

        if (_endDialogueWithTypeEvent != null)
            _endDialogueWithTypeEvent.RaiseEvent((int)_currentDialogue.DialogueType);

        _inputReader.AdvanceDialogueEvent -= OnAdvance;
        _gameState.UpdateGameState(GameState.Gameplay);

        //if (_gameState.CurrentGameState == GameState.Gameplay
        //    || _gameState.CurrentGameState == GameState.Combat)
            _inputReader.EnableGameplayInput();
    }
}