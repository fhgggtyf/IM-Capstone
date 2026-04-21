using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "new Journal Page", menuName = "Journals/Prologue Journal Page")]
public class PrologueJournalContentSO : ScriptableObject
{
    [SerializeField] private VideoClip _leftVideoClip = default;
    [SerializeField] private VideoClip _rightVideoClip = default;
    [SerializeField] private Sprite _LeftEnd = default;
    [SerializeField] private Sprite _RightEnd = default;
    [SerializeField] private bool _rightVideoSkippable = true;   // <-- ADD THIS
    [SerializeField] private VoidEventChannelSO _audioTrackEvent = default;
    [SerializeField] private VoidEventChannelSO _endOfPageEvent = default;
    [SerializeField] private StickerImageSO interactionImageSO = default;
    [SerializeField] private bool _isInteractable = false;

    public VoidEventChannelSO EndOfPageEvent => _endOfPageEvent;

    public VideoClip LeftVideoClip => _leftVideoClip;
    public VideoClip RightVideoClip => _rightVideoClip;
    public Sprite LeftEnd => _LeftEnd;
    public Sprite RightEnd => _RightEnd;
    public bool RightVideoSkippable => _rightVideoSkippable;      // <-- ADD THIS
    public StickerImageSO InteractionImageSO => interactionImageSO;
    public bool IsInteractable => _isInteractable;
}