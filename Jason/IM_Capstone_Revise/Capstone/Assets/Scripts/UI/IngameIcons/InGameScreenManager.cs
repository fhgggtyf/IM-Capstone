using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class InGameScreenManager : MonoBehaviour
{
    [SerializeField] private InputReader _reader;

    [SerializeField] private VoidEventChannelSO StartFirstStep;

    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioCueSO _newJournalNotificationSFX = default;

    [Header("Event Listeners")]
    [SerializeField] private VoidEventChannelSO QuestImageSwitchEvent;
    [SerializeField] private VoidEventChannelSO JournalImageSwitchEvent; // Second event channel

    [Header("UI References")]
    [SerializeField] private ImageSwitcher QuestImageSwitcher;
    [SerializeField] private ImageSwitcher JournalImageSwitcher;

    private void OnEnable()
    {
        if (QuestImageSwitchEvent != null)
            QuestImageSwitchEvent.OnEventRaised += OnQuestImageSwitchEvent;

        if (JournalImageSwitchEvent != null)
            JournalImageSwitchEvent.OnEventRaised += OnJournalImageSwitchEvent;

        StartFirstStep.RaiseEvent();
    }

    private void OnDisable()
    {
        if (QuestImageSwitchEvent != null)
            QuestImageSwitchEvent.OnEventRaised -= OnQuestImageSwitchEvent;

        if (JournalImageSwitchEvent != null)
            JournalImageSwitchEvent.OnEventRaised -= OnJournalImageSwitchEvent;
    }

    private void OnQuestImageSwitchEvent()
    {
        // Switch images on both referenced objects
        if (QuestImageSwitcher != null)
            QuestImageSwitcher.SwitchImage();

    }

    private void OnJournalImageSwitchEvent()
    {
        // Handle second event
        if (JournalImageSwitcher != null)
        {
            JournalImageSwitcher.SwitchImage();
            _sfxEventChannel.RaisePlayEvent(_newJournalNotificationSFX, _audioConfiguration);
            _reader.OpenJournalEvent += ResetJournalImage;
        }
    }

    private void ResetJournalImage()
    {
        JournalImageSwitcher.SwitchImage();
        _reader.OpenJournalEvent -= ResetJournalImage;
    }

}