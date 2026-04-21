using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTriggeredInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _isQuestItem = true; // Optional: Flag to indicate if this interactable is related to a quest
    [SerializeField] private bool _endsQuest = false; // Optional: Flag to indicate if interacting with this completes a quest
    [SerializeField] private StepSO _forStep; // Optional: Reference to the quest step this interactable is associated with (for editor organization)
    [SerializeField] private QuestManagerSO questManager; // Reference to your existing QuestManager scriptable object

    [Header("Quest Progress")]
    [SerializeField] private string _interactableId = "UniqueIdHere";
    [SerializeField] private StringEventChannelSO _objectInteractedEvent = default;

    [Header("Dialogue After Interaction")]
    [SerializeField] private DialogueDataSO _onInteractDialogue = default;          // <- 1 line or many lines live in this asset
    [SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;   // <- your existing channel that DialogueManager listens to

    [SerializeField] private bool _playDialogueOnce = true;
    private bool _hasPlayedDialogue;

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {

        Debug.Log($"Interacted with {gameObject.name} (ID: {_interactableId})");

        if (!_isQuestItem || questManager.CurrentStep != _forStep)
        {
            return; // If this interactable is not related to a quest, exit early
        }

        // 1) Notify quest system
        if (_objectInteractedEvent != null)
            _objectInteractedEvent.RaiseEvent(_interactableId);

        // 2) Play dialogue (optional)
        if (_onInteractDialogue != null && _startDialogueEvent != null)
        {
            if (!_playDialogueOnce || !_hasPlayedDialogue)
            {
                _startDialogueEvent.RaiseEvent(_onInteractDialogue);
                _hasPlayedDialogue = true;
            }
        }
    }
}
