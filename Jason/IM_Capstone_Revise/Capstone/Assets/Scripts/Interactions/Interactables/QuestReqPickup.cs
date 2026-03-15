using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class QuestReqPickup : MonoBehaviour, IInteractable<StepSO>
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [SerializeField] private OutlineController outlineController;

    [SerializeField] private VoidEventChannelSO _continueWithStep = default;

    [SerializeField] private StepSO ItemQuestData = default;

    public StepSO GetContext() => ItemQuestData;

    public void SetContext(StepSO context)
    {
        ItemQuestData = context;
    }

    public void EnableInteraction()
    {
        outlineController.EnableOutline();
    }

    public void DisableInteraction()
    {
        outlineController.DisableOutline();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        _continueWithStep.RaiseEvent();
        Destroy(gameObject);
    }

    private void Awake()
    {
        Rigidbody2D ??= GetComponent<Rigidbody2D>();

        if (ItemQuestData is null)
            return;
    }
}
