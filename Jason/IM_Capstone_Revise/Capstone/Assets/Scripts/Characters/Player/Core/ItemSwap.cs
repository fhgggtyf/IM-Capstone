using System;
using UnityEngine;

public class ItemSwap : CoreComponent
{
    private InteractableDetector interactableDetector;
    private ItemsInventory itemInventory;

    private ItemSO newItemData;
    private int count;

    private ItemPickup ItemPickup;

    private void HandleTryInteract(IInteractable interactable)
    {
        if (interactable is not ItemPickup pickup)
            return;

        ItemPickup = pickup;

        newItemData = ItemPickup.GetContext();
        count = ItemPickup.GetItemCount();


        Debug.Log("trying");
        if (itemInventory.TryGetEmptyIndex())
        {
            Debug.Log("Adding available");
            itemInventory.TryAddItem(newItemData, count);
            interactable.Interact();
            newItemData = null;
            return;
        }

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
        itemInventory = core.GetCoreComponent<ItemsInventory>();
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
