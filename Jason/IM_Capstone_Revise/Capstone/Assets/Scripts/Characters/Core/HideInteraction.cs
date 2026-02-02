using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        if (interactable is not HideInteractable hidable)
            return;

        _hideable = hidable;
        if (hidable.CanHide)
            _player.isHiding = !_player.isHiding;

        if (_player.isHiding)
        {
            _player.hideTarget = hidable.transform;
            interactableDetector.SetLockedInteractable(hidable);
        }
        else
        {
            _player.hideTarget = null;
            interactableDetector.SetLockedInteractable(null);
        }

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
