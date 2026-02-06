using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections;

public class VideoModuleController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button skipButton;

    [SerializeField] RawImage videoImage;
    [SerializeField] RenderTexture videoRT;

    [Header("Broadcasting")]
    [SerializeField] private VoidEventChannelSO _videoFinished;

    [Header("Listening To")]
    [SerializeField] private SOEventChannelSO _initializeVideoContent;

    void Awake()
    {
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRT;
        videoImage.texture = videoRT;
    }

    public void OnEnable()
    {
        if (_initializeVideoContent != null)
            _initializeVideoContent.OnEventRaised += PlayVideo;
    }

    public void OnDisable()
    {
        if (_initializeVideoContent != null)
            _initializeVideoContent.OnEventRaised -= PlayVideo;
    }

    private void PlayVideo(ScriptableObject videoClipSO)
    {
        if (videoClipSO is CGDataSO)
        {
            CGDataSO clipSO = videoClipSO as CGDataSO;
            Play(clipSO.videoClip, clipSO.skippable);
        }
        else
        {
            Debug.LogError("VideoModuleController received invalid VideoClipSO");
        }
    }

    public void Play(VideoClip clip, bool skippable)
    {
        Debug.Log("Playing video: " + clip.name);

        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnVideoFinished;

        skipButton.gameObject.SetActive(skippable);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(Finish);

        videoPlayer.Play();

        Debug.Log("Video audio tracks: " + videoPlayer.audioTrackCount);

    }

    private void OnVideoFinished(VideoPlayer vp) => Finish();

    private void Finish()
    {
        videoPlayer.Stop();
        _videoFinished.RaiseEvent();
        //gameObject.SetActive(false);
    }
}
