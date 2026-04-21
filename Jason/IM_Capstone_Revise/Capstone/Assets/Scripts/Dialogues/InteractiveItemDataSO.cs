using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "new Interactive Item", menuName = "Dialogues/Interactive Item Data")]
public class InteractiveItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    [Header("UI Layout Settings")]
    // Anchor presets
    public Vector2 anchorMin = new Vector2(0.5f, 0.5f);
    public Vector2 anchorMax = new Vector2(0.5f, 0.5f);

    // Pivot (optional, defaults to center)
    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    // Position relative to anchors (anchoredPosition)
    public Vector2 anchoredPosition = Vector2.zero;

    // Rotation (Euler angles in degrees)
    public Vector3 rotation = Vector3.zero;

    // Scale
    public Vector3 scale = Vector3.one;

    [Header("Dialogue")]
    public DialogueDataSO dialogue;
}
