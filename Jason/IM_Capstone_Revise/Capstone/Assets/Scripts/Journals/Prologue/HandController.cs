using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    [SerializeField] private BookNoFlipAnimation _book;
    [SerializeField] RectTransform handCursor;
    [SerializeField] Image pasteImage;
    [SerializeField] public VoidEventChannelSO ImageStampedEvent;

    private Face currentFace = new();

    // Start is called before the first frame update
    void OnEnable()
    {
        ImageStampedEvent.OnEventRaised += DisablePasteImg;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        int i = _book.CurrentPaper;

        if (i < 0 || i >= _book.papers.Length)
            return;

        handCursor.position = Input.mousePosition;
        pasteImage.transform.position = Input.mousePosition;

        if (currentFace != _book.papers[_book.CurrentPaper])
        {
            currentFace = _book.papers[_book.CurrentPaper];

            EnablePasteImg();

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

    private void OnDisable()
    {
        ImageStampedEvent.OnEventRaised -= DisablePasteImg;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
