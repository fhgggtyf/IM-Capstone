using UnityEngine;
using UnityEngine.Video;


#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "new Journal Page", menuName = "Journals/Prologue Journal Page")]
public class PrologueJournalContentSO : ScriptableObject
{
    [SerializeField] private VideoClip _leftVideoClip = default;
    [SerializeField] private VideoClip _rightVideoClip = default;
    [SerializeField] private VoidEventChannelSO _audioTrackEvent = default;
    [SerializeField] private VoidEventChannelSO _endOfPageEvent = default;
    [SerializeField] private StickerImageSO interactionImageSO = default;
    [SerializeField] private bool _isInteractable = false;

    public VoidEventChannelSO EndOfPageEvent => _endOfPageEvent;

    public VideoClip LeftVideoClip => _leftVideoClip;
    public VideoClip RightVideoClip => _rightVideoClip;
    public StickerImageSO InteractionImageSO => interactionImageSO;
    public bool IsInteractable => _isInteractable;

}

