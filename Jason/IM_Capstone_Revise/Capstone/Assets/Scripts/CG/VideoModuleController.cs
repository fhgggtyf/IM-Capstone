using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using System.Collections;

public class VideoModuleController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button skipButton;
    private Action _onFinished;

    public void Play(VideoClip clip, bool skippable, Action onFinished)
    {
        _onFinished = onFinished;

        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnVideoFinished;

        skipButton.gameObject.SetActive(skippable);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(Finish);

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp) => Finish();

    private void Finish()
    {
        videoPlayer.Stop();
        _onFinished?.Invoke();
        Destroy(gameObject);   // clean up module
    }
}
