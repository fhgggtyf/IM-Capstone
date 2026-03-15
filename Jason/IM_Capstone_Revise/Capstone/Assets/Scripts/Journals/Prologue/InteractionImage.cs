using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionImage : MonoBehaviour
{
    [SerializeField] private StickerImageSO stickerImageSO = default;

    [SerializeField] private HandController handController;
    [SerializeField] private GameObject objectToHide;

    [SerializeField] private BookNoFlipAnimation _book;

    public void OnButtonClick()
    {
        if (handController == null) return;
        if (handController.IsHolding) return;
        if (_book.papers[_book.currentPaper].Right.GetComponent<PrologueInteractionPageUI>().InteractionImage.gameObject.GetComponent<Image>().sprite != stickerImageSO.StickerSprite)
            return;

        handController.PickUp();

        if (objectToHide != null)
            objectToHide.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}