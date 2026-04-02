using UnityEngine;

[CreateAssetMenu(fileName = "new Journal Page", menuName = "Journals/Gameplay Journal Page")]
public class GameplayJournalContentSO : ScriptableObject
{
    [SerializeField] private Sprite _leftIMG = default;
    [SerializeField] private string _leftText = default;
    [SerializeField] private Sprite _rightIMG = default;
    [SerializeField] private string _rightText = default;

    public Sprite LeftIMG => _leftIMG;
    public Sprite RightIMG => _rightIMG;
    public string LeftText => _leftText;
    public string RightText => _rightText;

    // 或者添加一个初始化方法
    public void Initialize(Sprite leftImg, string leftText, Sprite rightImg, string rightText)
    {
        _leftIMG = leftImg;
        _leftText = leftText;
        _rightIMG = rightImg;
        _rightText = rightText;
    }
}