using UnityEngine;

public class QuestInteractable : InteractableItems
{
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
