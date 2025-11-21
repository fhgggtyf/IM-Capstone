using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "CG Data", menuName = "CG/CG Data")]
public class CGDataSO : ScriptableObject
{
    public string cgId;

    public VideoClip videoClip;
    public Sprite thumbnail;

    public string titleLocId;
    public string descriptionLocId;

    public bool autoPlay;
    public bool skippable = true;
    public bool canReplay = true;
}
