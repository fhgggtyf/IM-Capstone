using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIDialogueManager : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent _dialogueLineText = default;
    [SerializeField] private LocalizeStringEvent _narrationLineText = default;

    [SerializeField] private Image _actorPortrait = default;

    [SerializeField] private GameObject _dialoguePanel = default;
    [SerializeField] private GameObject _noActorNarrationPanel = default;

    //[SerializeField] private LocalizeStringEvent _actorNameText = default;
    //[SerializeField] private LocalizeStringEvent _recievantNameText = default;

    //[SerializeField] private GameObject _actorNamePanel = default;
    //[SerializeField] private GameObject _mainProtagonistNamePanel = default;
    [SerializeField] private UIDialogueChoicesManager _choicesManager = default;

	[Header("Listening to")]
	[SerializeField] private DialogueChoicesChannelSO _showChoicesEvent = default;

    private LocalizeStringEvent _lineText;
    public Func<StepSO, bool> IsQuestCompleted { get; set; }

    private void OnEnable()
	{
		_showChoicesEvent.OnEventRaised += ShowChoices;
	}

	private void OnDisable()
	{
		_showChoicesEvent.OnEventRaised -= ShowChoices;
	}

	public void SetDialogue(LocalizedString dialogueLine, ActorSO actor, bool isMainProtagonist)
	{
        switch (actor.ActorId) { 
            case ActorID.NAR:
                _dialoguePanel.SetActive(false);
                _noActorNarrationPanel.SetActive(true);
                _lineText = _narrationLineText;
                ShowNarration(dialogueLine);
                break;
            default:
                _dialoguePanel.SetActive(true);
                _noActorNarrationPanel.SetActive(false);
                _lineText = _dialogueLineText;
                ShowDialogue(actor, dialogueLine);
                break;
        }
    }

    private void ShowDialogue(ActorSO actor, LocalizedString dialogueLine)
    {
        _choicesManager.gameObject.SetActive(false);
        _lineText.StringReference = dialogueLine;

        Sprite portraitKey;

        // 直接传入 StepSO 对象，委托内部判断这个 Step 是否完成
        Debug.Log(actor.MeetStep != null && IsQuestCompleted != null && IsQuestCompleted(actor.MeetStep));
        if (actor.MeetStep != null && IsQuestCompleted != null && IsQuestCompleted(actor.MeetStep))
        {
            portraitKey = actor.ActorPortrait;
        }
        else
        {
            portraitKey = actor.ActorPortraitUnknown != null
                ? actor.ActorPortraitUnknown
                : actor.ActorPortrait;
        }

        _actorPortrait.sprite = portraitKey;
    }

    private void ShowNarration(LocalizedString dialogueLine)
    {
        _choicesManager.gameObject.SetActive(false);
        _lineText.StringReference = dialogueLine;
    }

    private void ShowChoices(List<Choice> choices)
	{
		_choicesManager.FillChoices(choices);
		_choicesManager.gameObject.SetActive(true);
	}

	private void HideChoices()
	{
		_choicesManager.gameObject.SetActive(false);
	}
}
