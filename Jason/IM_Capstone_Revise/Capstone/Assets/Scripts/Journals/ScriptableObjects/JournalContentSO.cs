using UnityEngine;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "new Journal Page", menuName = "Journals/Journal Page")]
public class JournalContentSO : ScriptableObject
{
    [SerializeField] private LocalizedString _text = default;
    [SerializeField] private LocalizedString _date = default;
    [SerializeField] private LocalizedString _time = default;
    [SerializeField] private Sprite _image = default;
    [SerializeField] private VoidEventChannelSO _audioTrackEvent = default;
    [SerializeField] private VoidEventChannelSO _endOfPageEvent = default;

    public VoidEventChannelSO EndOfPageEvent => _endOfPageEvent;

    public LocalizedString Text => _text;
    public LocalizedString Date => _date;
    public LocalizedString Time => _time;
    public Sprite Image => _image;
}

