using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new journal sticker", menuName = "Journals/sticker")]
public class StickerImageSO : ScriptableObject
{
    [SerializeField] private Sprite _stickerSprite = default;

    public Sprite StickerSprite => _stickerSprite;
}
