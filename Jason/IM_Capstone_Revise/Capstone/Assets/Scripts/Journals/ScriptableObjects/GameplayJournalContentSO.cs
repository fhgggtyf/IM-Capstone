using UnityEngine;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization;
using UnityEngine.Video;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "new Journal Page", menuName = "Journals/Gameplay Journal Page")]
public class GameplayJournalContentSO : ScriptableObject
{
    [SerializeField] private Image _leftIMG = default;
    [SerializeField] private string _leftText = default;
    [SerializeField] private Image _rightIMG = default;
    [SerializeField] private string _rightText = default;

    public Image LeftIMG => _leftIMG;
    public Image RightIMG => _rightIMG;
    public string LeftText => _leftText;
    public string RightText => _rightText;

}

