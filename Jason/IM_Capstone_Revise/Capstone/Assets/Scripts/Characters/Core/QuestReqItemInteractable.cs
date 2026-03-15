using System;
using UnityEngine;

public class QuestReqItemInteractable: CoreComponent
{
    private InteractableDetector interactableDetector;

    private void HandleTryInteract(IInteractable interactable)
    {
        if (interactable is not QuestReqPickup)
        {
            Debug.Log("Not QRP");
            return;
        }

        interactable.Interact();

        //OnChoiceRequested?.Invoke(new WeaponSwapChoiceRequest(
        //    HandleWeaponSwapChoice,
        //    weaponInventory.GetWeaponSwapChoices(),
        //    newWeaponData
        //));
        //
        //Prompt no more space
    }

    //private void HandleWeaponSwapChoice(WeaponSwapChoice choice)
    //{
    //    if (!weaponInventory.TrySetWeapon(newWeaponData, choice.Index, out var oldData))
    //        return;

    //    newWeaponData = null;

    //    OnWeaponDiscarded?.Invoke(oldData);

    //    if (weaponPickup is null)
    //        return;

    //    weaponPickup.Interact();

    //}

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
