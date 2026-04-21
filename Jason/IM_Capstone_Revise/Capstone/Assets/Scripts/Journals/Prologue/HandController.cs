using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    [SerializeField] private BookNoFlipAnimation _book;
    [SerializeField] RectTransform handCursor;
    [SerializeField] Image pasteImage;

    private bool _isHolding;

    private Sprite _heldSprite;  // new field

    public bool IsHolding => _isHolding;

    private Face currentFace = new();

    // Start is called before the first frame update
    void OnEnable()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        // Always follow mouse
        handCursor.position = Input.mousePosition;
        pasteImage.transform.position = Input.mousePosition;

        // Only sync sprite from current page when NOT holding
        if (!_isHolding)
        {
            int i = _book.CurrentPaper;
            if (i >= 0 && i < _book.papers.Length)
            {
                var current = _book.papers[i];
                if (current != currentFace)
                {
                    currentFace = current;
                    var sourceImg = currentFace.Right.GetComponent<PrologueInteractionPageUI>().InteractionImage;
                    pasteImage.sprite = sourceImg.sprite;

                    var targetRect = pasteImage.rectTransform;
                    var sourceRect = sourceImg.rectTransform;

                    targetRect.SetSizeWithCurrentAnchors(
                        RectTransform.Axis.Horizontal,
                        sourceRect.rect.width);

                    targetRect.SetSizeWithCurrentAnchors(
                        RectTransform.Axis.Vertical,
                        sourceRect.rect.height
                    );
                }
            }
        }

    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PickUp(Sprite stickerSprite)  // change signature
    {
        _isHolding = true;
        _heldSprite = stickerSprite;
        pasteImage.sprite = _heldSprite;
        EnablePasteImg();
    }

    public void DropDown()
    {
        _isHolding = false;
        _heldSprite = null;
        DisablePasteImg();
    }

    void DisablePasteImg()
    {
        pasteImage.gameObject.SetActive(false);
    }
    void EnablePasteImg()
    {
        pasteImage.gameObject.SetActive(true);
    }
}
