using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VideoURLMap", menuName = "Video/Map")]
public class VideoUrlMap : ScriptableObject
{
    public List<VideoMapping> mappings;
}

[System.Serializable]
public class VideoMapping
{
    public VideoClip clip;
    public string streamingUrl; // e.g., "Videos/Prologue/video1.mp4"
}