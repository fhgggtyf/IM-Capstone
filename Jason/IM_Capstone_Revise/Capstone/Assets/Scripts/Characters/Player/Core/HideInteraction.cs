using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInteraction : CoreComponent
{
    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    private InteractableDetector interactableDetector;
    private HideInteractable _hideable;

    [SerializeField] private Player _player;

    public HideInteractable Climbable { get => _hideable; set => _hideable = value; }

    private void HandleTryInteract(IInteractable interactable)
    {
        if (interactable is not HideInteractable climbable)
            return;

        _hideable = climbable;

        _player.isHiding = !_player.isHiding;
    }

    protected override void Awake()
    {
        base.Awake();

        interactableDetector = core.GetCoreComponent<InteractableDetector>();
    }

    private void OnEnable()
    {
        interactableDetector.OnTryInteract += HandleTryInteract;
    }


    private void OnDisable()
    {
        interactableDetector.OnTryInteract -= HandleTryInteract;
    }
}
