using UnityEditor;
using UnityEngine;

public enum StepType
{
    Dialogue,
    GiveItem,
    CheckItem
}

[CreateAssetMenu(fileName = "step", menuName = "Quests/Step")]
public class StepSO : SerializableScriptableObject
{
    [Tooltip("The Character this mission will need interaction with")]
    [SerializeField] private ActorSO _actor = default;

    [Tooltip("The dialogue that will be displayed before an action, if any")]
    [SerializeField] private DialogueDataSO _dialogueBeforeStep = default;

    [Tooltip("The dialogue that will be displayed when the step is achieved")]
    [SerializeField] private DialogueDataSO _completeDialogue = default;

    [Tooltip("The dialogue that will be displayed if the step is not achieved yet")]
    [SerializeField] private DialogueDataSO _incompleteDialogue = default;

    [SerializeField] private StepType _type = default;

    [Tooltip("The item to check/give")]
    [SerializeField] private ItemSO _item = default;

    [SerializeField] private bool _hasReward = default;

    [Tooltip("The item to reward if any")]
    [SerializeField] private ItemSO _rewardItem = default;

    [SerializeField] private int _rewardItemCount = 1;

    [SerializeField] private bool _isDone = false;

    [SerializeField] private VoidEventChannelSO _endStepEvent = default;


    // ============================
    //      PUBLIC PROPERTIES
    // ============================

    public ActorSO Actor => _actor;

    public DialogueDataSO DialogueBeforeStep
    {
        get => _dialogueBeforeStep;
        set => _dialogueBeforeStep = value;
    }

    public DialogueDataSO CompleteDialogue
    {
        get => _completeDialogue;
        set => _completeDialogue = value;
    }

    public DialogueDataSO IncompleteDialogue
    {
        get => _incompleteDialogue;
        set => _incompleteDialogue = value;
    }

    public StepType Type => _type;

    public ItemSO Item
    {
        get => _item;
        set => _item = value;
    }

    public bool HasReward => _hasReward;

    public ItemSO RewardItem => _rewardItem;

    public int RewardItemCount => _rewardItemCount;

    public VoidEventChannelSO EndStepEvent
    {
        get => _endStepEvent;
        set => _endStepEvent = value;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }


    // ============================
    //      METHODS
    // ============================

    public void FinishStep()
    {
        if (_endStepEvent != null)
            _endStepEvent.RaiseEvent();

        _isDone = true;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Only for Questline Tool in Editor to remove a Step
    /// </summary>
    public string GetPath()
    {
        return AssetDatabase.GetAssetPath(this);
    }
#endif
}
