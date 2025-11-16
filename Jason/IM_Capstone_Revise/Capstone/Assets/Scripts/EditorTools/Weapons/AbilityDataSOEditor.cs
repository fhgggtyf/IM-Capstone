// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(AbilityDataSO))]
// public class AbilityDataSOEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         // Get a reference to the target object
//         AbilityDataSO abilityData = (AbilityDataSO)target;

//         // Draw the default inspector excluding canBeInterupted
//         DrawDefaultInspectorExcept("canBeInterupted");

//         // Conditionally show the canBeInterupted field based on IsEnemyAbility
//         if (abilityData.IsEnemyAbility)
//         {
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("canBeInterupted"));
//         }

//         // Apply any changes to the serialized object
//         serializedObject.ApplyModifiedProperties();
//     }

//     private void DrawDefaultInspectorExcept(string propertyName)
//     {
//         // Start checking for changes in the inspector
//         serializedObject.Update();

//         // Iterate over all properties and draw them unless they match the specified name
//         SerializedProperty property = serializedObject.GetIterator();
//         if (property.NextVisible(true))
//         {
//             do
//             {
//                 if (property.name != propertyName)
//                 {
//                     EditorGUILayout.PropertyField(property, true);
//                 }
//             }
//             while (property.NextVisible(false));
//         }

//         // Apply any changes to the serialized object
//         serializedObject.ApplyModifiedProperties();
//     }
// }
