using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using TMPro;

[System.Serializable]
public class PopAnimationSettings
{
    [Header("Position Settings")]
    [SerializeField] public Vector2 startOffsetMin = new Vector2(0, 0);
    [SerializeField] public Vector2 startOffsetMax = new Vector2(0, 0);
    [SerializeField] public Vector2 targetOffsetMin = new Vector2(-100, -50);
    [SerializeField] public Vector2 targetOffsetMax = new Vector2(100, 50);

    [Header("Text Animation Settings")]
    [SerializeField] public Color textStartColor = Color.white;
    [SerializeField] public Color textTargetColor = Color.white;
    [SerializeField] public float textStartAlpha = 0f;
    [SerializeField] public float textTargetAlpha = 1f;
    [SerializeField] public Vector2 textStartScale = Vector2.one;
    [SerializeField] public Vector2 textTargetScale = Vector2.one;
    [SerializeField] public float textStartFontSize = 14f;
    [SerializeField] public float textTargetFontSize = 18f;

    [Header("Timing Settings")]
    public float moveDuration = 0.5f;
    public float holdDuration = 1.5f;
    public float returnDuration = 0.5f;

    [Header("Easing Settings")]
    public LeanTweenType moveEase = LeanTweenType.easeOutBack;
    public LeanTweenType returnEase = LeanTweenType.easeInOutSine;
}

public class QuestBoxPop : MonoBehaviour
{
    [SerializeField] private InputReader _reader;
    [SerializeField] private QuestManagerSO _questManager;
    [SerializeField] private LocalizedString _questDetail;
    [SerializeField] private StepSO _defaultStep;
    [SerializeField] private TMP_Text _questText;

    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioCueSO _QuestBoxSFX = default;

    [Header("Animation Settings")]
    [SerializeField] private PopAnimationSettings animationSettings = new PopAnimationSettings();

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO NewStepRecieved;

    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO ChangeQuestIcon;

    [Header("Target UI Element")]
    [SerializeField] private RectTransform targetRectTransform;

    private RectOffset originalOffsets;
    private Color originalTextColor;
    private float originalFontSize;
    private Vector3 originalTextScale;
    private bool isAnimating = false;
    private LTDescr currentTween;

    private void OnEnable()
    {
        if (NewStepRecieved != null)
        {
            NewStepRecieved.OnEventRaised += OnTriggerEvent;
        }
        _reader.ReadQuestEvent += OnTriggerEvent;
    }

    private void OnDisable()
    {
        if (NewStepRecieved != null)
        {
            NewStepRecieved.OnEventRaised -= OnTriggerEvent;
        }

        // Clean up any ongoing tweens
        if (currentTween != null)
        {
            LeanTween.cancel(currentTween.id);
        }
        _reader.ReadQuestEvent -= OnTriggerEvent;
    }

    private void Start()
    {
        if (targetRectTransform == null)
        {
            targetRectTransform = GetComponent<RectTransform>();
        }

        if (targetRectTransform != null)
        {
            // Store original offsets
            originalOffsets = new RectOffset(
                (int)targetRectTransform.offsetMin.x,
                (int)targetRectTransform.offsetMin.y,
                (int)targetRectTransform.offsetMax.x,
                (int)targetRectTransform.offsetMax.y
            );

            // Set to start position
            SetRectOffsets(animationSettings.startOffsetMin, animationSettings.startOffsetMax);
        }

        // Store original text properties
        if (_questText != null)
        {
            originalTextColor = _questText.color;
            originalFontSize = _questText.fontSize;
            originalTextScale = _questText.transform.localScale;

            // Set initial text state
            SetTextState(0f);
        }
    }

    private void Update()
    {
        if (_questManager.CurrentStep == null)
        {
            _questDetail = _defaultStep.StepDescription;
        }
        else
        {
            _questDetail = _questManager.CurrentStep.StepDescription;
        }

        if (_questText != null)
        {
            _questText.text = _questDetail.GetLocalizedString();
        }
    }

    private void OnTriggerEvent()
    {
        if (isAnimating) return;

        _sfxEventChannel.RaisePlayEvent(_QuestBoxSFX, _audioConfiguration);
        ChangeQuestIcon.RaiseEvent();
        StartCoroutine(PopAnimationRoutine());
    }

    private IEnumerator PopAnimationRoutine()
    {
        isAnimating = true;

        if (targetRectTransform == null) yield break;

        // Move UI from start to target
        currentTween = LeanTween.value(gameObject, 0f, 1f, animationSettings.moveDuration)
            .setEase(animationSettings.moveEase)
            .setOnUpdate((float t) => {
                // Animate UI position
                Vector2 currentMin = Vector2.Lerp(
                    animationSettings.startOffsetMin,
                    animationSettings.targetOffsetMin,
                    t
                );
                Vector2 currentMax = Vector2.Lerp(
                    animationSettings.startOffsetMax,
                    animationSettings.targetOffsetMax,
                    t
                );
                SetRectOffsets(currentMin, currentMax);

                // Animate text properties
                SetTextState(t);
            });

        // Wait at target position
        yield return new WaitForSeconds(animationSettings.moveDuration + animationSettings.holdDuration);

        // Move back to start position
        currentTween = LeanTween.value(gameObject, 0f, 1f, animationSettings.returnDuration)
            .setEase(animationSettings.returnEase)
            .setOnUpdate((float t) => {
                // Reverse animation for UI
                Vector2 currentMin = Vector2.Lerp(
                    animationSettings.targetOffsetMin,
                    animationSettings.startOffsetMin,
                    t
                );
                Vector2 currentMax = Vector2.Lerp(
                    animationSettings.targetOffsetMax,
                    animationSettings.startOffsetMax,
                    t
                );
                SetRectOffsets(currentMin, currentMax);

                // Reverse animation for text
                SetTextState(1f - t);
            });

        // Wait for return to complete
        yield return new WaitForSeconds(animationSettings.returnDuration);

        ChangeQuestIcon.RaiseEvent();
        isAnimating = false;
        currentTween = null;
    }

    private void SetTextState(float progress)
    {
        if (_questText == null) return;

        // Animate color
        Color targetColor = Color.Lerp(animationSettings.textStartColor,
            animationSettings.textTargetColor, progress);
        float alpha = Mathf.Lerp(animationSettings.textStartAlpha,
            animationSettings.textTargetAlpha, progress);
        _questText.color = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);

        // Animate scale
        Vector2 scale = Vector2.Lerp(animationSettings.textStartScale,
            animationSettings.textTargetScale, progress);
        _questText.transform.localScale = new Vector3(scale.x, scale.y, 1f);

        // Animate font size
        _questText.fontSize = Mathf.Lerp(animationSettings.textStartFontSize,
            animationSettings.textTargetFontSize, progress);
    }

    private void SetRectOffsets(Vector2 offsetMin, Vector2 offsetMax)
    {
        if (targetRectTransform == null) return;

        targetRectTransform.offsetMin = offsetMin;
        targetRectTransform.offsetMax = offsetMax;
    }

    // Public method to trigger animation manually
    public void TriggerAnimation()
    {
        if (!isAnimating && targetRectTransform != null)
        {
            StartCoroutine(PopAnimationRoutine());
        }
    }

    // Public method to reset to start position
    public void ResetToStart()
    {
        if (currentTween != null)
        {
            LeanTween.cancel(currentTween.id);
        }

        SetRectOffsets(animationSettings.startOffsetMin, animationSettings.startOffsetMax);
        SetTextState(0f);
        isAnimating = false;
        currentTween = null;
    }
}