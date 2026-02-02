using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableInteractions : CoreComponent
{
    private InteractableDetector interactableDetector;

    private void HandleTryInteract(IInteractable interactable)
    {
        if (interactable is not InteractableItems)
            return;

        interactable.Interact();
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
