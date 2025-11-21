using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StepSO))]
public class StepSOEditor : Editor
{
    SerializedProperty actor;
    SerializedProperty dialogueBefore;
    SerializedProperty completeDialogue;
    SerializedProperty incompleteDialogue;
    SerializedProperty journalData;
    SerializedProperty cutsceneLocation;
    SerializedProperty cutsceneEvent;
    SerializedProperty cgData;
    SerializedProperty stepType;
    SerializedProperty item;
    SerializedProperty hasReward;
    SerializedProperty rewardItem;
    SerializedProperty rewardItemCount;
    SerializedProperty endStepEvent;
    SerializedProperty isDone;

    private void OnEnable()
    {
        actor = serializedObject.FindProperty("_actor");
        dialogueBefore = serializedObject.FindProperty("_dialogueBeforeStep");
        completeDialogue = serializedObject.FindProperty("_completeDialogue");
        incompleteDialogue = serializedObject.FindProperty("_incompleteDialogue");
        journalData = serializedObject.FindProperty("_journalData");
        cutsceneLocation = serializedObject.FindProperty("_loadCutsceneLocationEvent");
        cutsceneEvent = serializedObject.FindProperty("_cutsceneEvent");
        cgData = serializedObject.FindProperty("_cgData");
        stepType = serializedObject.FindProperty("_type");
        item = serializedObject.FindProperty("_item");
        hasReward = serializedObject.FindProperty("_hasReward");
        rewardItem = serializedObject.FindProperty("_rewardItem");
        rewardItemCount = serializedObject.FindProperty("_rewardItemCount");
        endStepEvent = serializedObject.FindProperty("_endStepEvent");
        isDone = serializedObject.FindProperty("_isDone");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(stepType);

        StepType type = (StepType)stepType.enumValueIndex;

        EditorGUILayout.Space(10);

        // Optional: show Actor for relevant step types
        if (type == StepType.Dialogue || type == StepType.GiveItem || type == StepType.CheckItem)
        {
            EditorGUILayout.LabelField("Actor", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(actor);
            EditorGUILayout.Space(5);
        }

        switch (type)
        {
            case StepType.Dialogue:
                EditorGUILayout.LabelField("Dialogue Block", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(dialogueBefore);
                EditorGUILayout.PropertyField(completeDialogue);
                EditorGUILayout.PropertyField(incompleteDialogue);
                break;

            case StepType.GiveItem:
                EditorGUILayout.LabelField("Item Transaction", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(item);
                EditorGUILayout.PropertyField(hasReward);
                if (hasReward.boolValue)
                {
                    EditorGUILayout.PropertyField(rewardItem);
                    EditorGUILayout.PropertyField(rewardItemCount);
                }
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(completeDialogue);
                break;

            case StepType.CheckItem:
                EditorGUILayout.LabelField("Item Check", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(item);
                EditorGUILayout.PropertyField(incompleteDialogue);
                EditorGUILayout.PropertyField(completeDialogue);
                break;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(isDone);

        serializedObject.ApplyModifiedProperties();
    }
}
