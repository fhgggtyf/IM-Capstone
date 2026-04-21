using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveItem : MonoBehaviour
{
    public InteractiveItemDataSO InteractiveItemDataSO;
    public Image ImageComp;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO _onClickEvent = default;
    [SerializeField] private DialogueDataChannelSO _narrationDataChannel = default;

    private void OnEnable()
    {
        ImageComp.sprite = InteractiveItemDataSO.itemSprite;
        ApplyRectTransformSettings(GetComponent<RectTransform>(),
            InteractiveItemDataSO.anchorMin, InteractiveItemDataSO.anchorMax,
            InteractiveItemDataSO.pivot,
            InteractiveItemDataSO.anchoredPosition,
            InteractiveItemDataSO.rotation,
            InteractiveItemDataSO.scale);
    }

    public static void ApplyRectTransformSettings(RectTransform rectTransform,
    Vector2 anchorMin, Vector2 anchorMax,
    Vector2 pivot,
    Vector2 anchoredPosition,
    Vector3 rotation,
    Vector3 scale)
    {
        // Set anchors
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Set pivot (point around which position/rotation/scale are applied)
        rectTransform.pivot = pivot;

        // Set position relative to anchors
        rectTransform.anchoredPosition = anchoredPosition;

        // Set rotation (using localEulerAngles preserves parent rotation)
        rectTransform.localEulerAngles = rotation;

        // Set scale
        rectTransform.localScale = scale;
    }

    public void OnClick()
    {
        if(_onClickEvent != null)
            _onClickEvent.RaiseEvent();
        if(_narrationDataChannel != null)
            _narrationDataChannel.RaiseEvent(InteractiveItemDataSO.dialogue);
    }
}
