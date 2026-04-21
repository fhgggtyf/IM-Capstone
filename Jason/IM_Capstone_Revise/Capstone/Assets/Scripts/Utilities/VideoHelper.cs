using UnityEngine;
using UnityEngine.Video;
using System.IO;

public static class VideoHelper
{
    private static VideoUrlMap _map;
    private static VideoUrlMap Map
    {
        get
        {
            if (_map == null)
                _map = Resources.Load<VideoUrlMap>("Config/VideoUrlMap"); // path without extension
            return _map;
        }
    }

    public static void AssignVideo(VideoPlayer player, VideoClip clip)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        player.clip = clip;
        player.source = VideoSource.VideoClip;
#else
        string url = GetUrlForClip(clip);
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError($"No streaming URL found for clip {clip.name}", clip);
            return;
        }
        player.source = VideoSource.Url;
        player.url = Path.Combine(Application.streamingAssetsPath, url);
#endif
    }

    private static string GetUrlForClip(VideoClip clip)
    {
        foreach (var mapping in Map.mappings)
            if (mapping.clip == clip)
                return mapping.streamingUrl;
        return null;
    }
}