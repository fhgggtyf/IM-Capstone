#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestManagerSO))]
public class QuestManagerSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw your normal inspector first
        DrawDefaultInspector();

        var qm = (QuestManagerSO)target;

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("Runtime (Play Mode)", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField("Current Questline", qm.CurrentQuestline, typeof(QuestlineSO), false);
            EditorGUILayout.ObjectField("Current Quest", qm.CurrentQuest, typeof(QuestSO), false);
            EditorGUILayout.ObjectField("Current Step", qm.CurrentStep, typeof(StepSO), false);

            EditorGUILayout.IntField("Questline Index", qm.CurrentQuestlineIndex);
            EditorGUILayout.IntField("Quest Index", qm.CurrentQuestIndex);
            EditorGUILayout.IntField("Step Index", qm.CurrentStepIndex);
        }

        // Force repaint during play so it updates ¡°live¡±
        if (Application.isPlaying)
            Repaint();
    }
}
#endif
