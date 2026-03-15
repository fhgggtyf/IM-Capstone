using UnityEngine;

public class QuestInteractable : InteractableItems
{
    [SerializeField]private bool _isQuestItem = true; // Optional: Flag to indicate if this interactable is related to a quest

    [Header("Quest Progress")]
    [SerializeField] private string _interactableId = "UniqueIdHere";
    [SerializeField] private StringEventChannelSO _objectInteractedEvent = default;

    [Header("Dialogue After Interaction")]
    [SerializeField] private DialogueDataSO _onInteractDialogue = default;          // <- 1 line or many lines live in this asset
    [SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;   // <- your existing channel that DialogueManager listens to

    [SerializeField] private bool _playDialogueOnce = true;
    private bool _hasPlayedDialogue;

    public override void Interact()
    {

        Debug.Log($"Interacted with {gameObject.name} (ID: {_interactableId})");

        if (!_isQuestItem)
        {
            return; // If this interactable is not related to a quest, exit early
        }

        base.Interact();

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
