using System.Collections.Generic;
using UnityEngine;

public class PrologueRightSideInteractionController : MonoBehaviour
{
    public List<PrologueInteractionPageUI> interactionPages;
    public InputReader inputReader;

    [SerializeField] public VoidEventChannelSO ImageStampedEvent;
    [SerializeField] private HandController handController;
    [SerializeField] private BookNoFlipAnimation book;                // <-- ADD

    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
    [SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
    [SerializeField] private AudioCueSO _journalStickSFX = default;

    void Update()
    {
        foreach (PrologueInteractionPageUI page in interactionPages)
        {
            if (page.gameObject.GetComponent<CanvasGroup>().alpha == 1)
            {
                page.InteractionButton.interactable = true;
                page.InteractionButton.onClick.RemoveAllListeners();
                page.InteractionButton.onClick.AddListener(() => {
                    // 1. Must be holding a sticker
                    if (!handController.IsHolding) return;

                    _sfxEventChannel.RaisePlayEvent(_journalStickSFX, _audioConfiguration);

                    // 2. Get current page data
                    int currentIndex = book.CurrentPaper;
                    if (currentIndex < 0 || currentIndex >= book.papers.Length) return;
                    PrologueJournalContentSO pageData = book.papers[currentIndex].pageData;
                    if (pageData == null || pageData.RightVideoClip == null)
                    {
                        Debug.LogWarning("No right video clip on current page.");
                        return;
                    }

                    // 3. Play the video
                    page.videoController.gameObject.SetActive(true);
                    page.videoController.Play(pageData.RightVideoClip, pageData.RightVideoSkippable);

                    //// 4. Place the sticker (show image, hide button)
                    //page.EndImage.gameObject.SetActive(true);
                    page.InteractionButton.gameObject.SetActive(false);

                    // 5. Drop the sticker from hand
                    handController.DropDown();

                    // 6. Restore input and raise event (if needed)
                    inputReader.EnableJournalInput();
                    ImageStampedEvent?.RaiseEvent();
                });
            }
            else
            {
                page.InteractionButton.interactable = false;
                page.InteractionButton.onClick.RemoveAllListeners();
            }
        }
    }

}