using UnityEngine;

/// <summary>
/// Adapter that makes an NPC usable by the "InteractableDetector + IInteractable" system.
/// Put this on the NPC (or on the same GameObject as the collider that enters the player's detector).
/// </summary>
public class NPCInteractable : InteractableItems
{
    [Header("References")]
    [SerializeField] private StepController _stepController;
    [SerializeField] private NPC _npc; // optional, only used if you want to set npcState=Talk

    [Header("Optional feedback")]
    [Tooltip("Optional visual/prompt to show when player is in range (outline, icon, canvas prompt, etc.).")]
    [SerializeField] private GameObject _interactionHint;

    [Header("Behavior")]
    [Tooltip("If true, sets NPC.npcState = Talk when interacting.")]
    [SerializeField] private bool _setTalkStateOnInteract = true;

    private void Awake()
    {
        // Auto-wire if not set (supports placing this on a child collider)
        if (_stepController == null) _stepController = GetComponentInParent<StepController>();
        if (_npc == null) _npc = GetComponentInParent<NPC>();

        if (_interactionHint != null)
            _interactionHint.SetActive(false);
    }

    public void ActivateIndicator()
    {
        if (_interactionHint != null)
            _interactionHint.SetActive(true);
    }

    public void DeactivateIndicator()
    {
        if (_interactionHint != null)
            _interactionHint.SetActive(false);
    }

    public override void Interact()
    {
        if (_setTalkStateOnInteract && _npc != null)
            _npc.npcState = NPCState.Talk;

        if (_stepController == null)
        {
            Debug.LogWarning($"[{nameof(NPCInteractable)}] No StepController found on {name} or its parents.");
            return;
        }

        ActivateIndicator();

        _stepController.InteractWithCharacter();
    }
}
