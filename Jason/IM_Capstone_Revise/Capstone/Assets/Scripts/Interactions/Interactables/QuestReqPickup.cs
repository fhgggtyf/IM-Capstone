using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class QuestReqPickup : MonoBehaviour, IInteractable<StepSO>
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [SerializeField] private QuestManagerSO questManager;

    [SerializeField] private OutlineController outlineController;

    [SerializeField] private VoidEventChannelSO _continueWithStep = default;
    [SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

    [SerializeField] private StepSO ItemQuestData = default;
    [SerializeField] private bool _destroyOnInteract = true;

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
        if (ItemQuestData.Type == StepType.Dialogue)
        {
            _startDialogueEvent.RaiseEvent(ItemQuestData.DialogueBeforeStep);
        }
        else
        {
            _continueWithStep.RaiseEvent();
        }

        if (_destroyOnInteract)
            Destroy(gameObject);
    }

    private void Awake()
    {
        if (ItemQuestData is null)
            return;

        if(questManager.CurrentStep == ItemQuestData)
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
