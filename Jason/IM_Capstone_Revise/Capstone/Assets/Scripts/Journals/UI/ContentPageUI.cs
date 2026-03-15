using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class ContentPageUI : MonoBehaviour
{
    [Header("Left Side UI")]
    public VideoPlayer videoPlayer;

    public void ShowAnimation()
    {
        videoPlayer.Play();
        Debug.Log("Playing videoclip: " + videoPlayer.clip);
    }

    // If later you want Right side UI, add it here too
    // public TMP_Text backText; etc.
}
