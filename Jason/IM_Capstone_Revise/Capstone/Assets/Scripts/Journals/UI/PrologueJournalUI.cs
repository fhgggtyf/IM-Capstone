using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PrologueJournalUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BookNoFlipAnimation book;                // The BookPro flipping system
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject interactionPagePrefab;

    [Header("Listening On")]
    [SerializeField] BoolEventChannelSO FlipToLeft;
    [SerializeField] IntEventChannelSO UnlockPages;

    private JournalDataSO _journalData;

    private void OnEnable()
    {
        UnlockPages.OnEventRaised += ChangeBookPageAccess;
        FlipToLeft.OnEventRaised += Flip;

        book.Initialize();
        book.CurrentPaper = 0;
    }

    private void OnDisable()
    {
        UnlockPages.OnEventRaised -= ChangeBookPageAccess;
        FlipToLeft.OnEventRaised -= Flip;
    }

    // ======================================================================
    // INITIALIZATION (CALLED BY JOURNAL MANAGER)
    // ======================================================================
    public void Initialize(JournalDataSO data)
    {
        _journalData = data;

        CreatePagesForBook();

    }

    // ======================================================================
    // CREATE PAGE INSTANCES & ASSIGN INTO BookPro
    // ======================================================================

    public void AddPaper(GameObject LeftPage, GameObject RightPage, PrologueJournalContentSO journalContent)
    {
        Debug.Log("Adding a new paper to the book.");
        LeftPage.transform.SetParent(book.LeftPageTransform, false);
        RightPage.transform.SetParent(book.RightPageTransform, false);
        book.RightPageTransform.gameObject.GetComponent<PrologueRightSideInteractionController>().interactionPages.Add(RightPage.GetComponent<PrologueInteractionPageUI>());
        Face newPaper = new Face();
        newPaper.Left = LeftPage;
        newPaper.Right = RightPage;
        newPaper.IsInteractable = journalContent.IsInteractable; // Example condition for interactability
        newPaper.pageData = journalContent;   // <-- ADD THIS LINE
        Face[] papers = new Face[book.papers.Length + 1];
        for (int i = 0; i < book.papers.Length; i++)
        {
            papers[i] = book.papers[i];
        }
        papers[papers.Length - 1] = newPaper;
        book.papers = papers;
        //update the flipping range to contain the new added paper
        //book.EndFlippingPaper = book.papers.Length - 1;
        book.UpdatePages();
    }
    private void CreatePagesForBook()
    {
        List<PrologueJournalContentSO> pages = _journalData.Pages;
        int count = pages.Count;
        Debug.Log($"Creating {count} journal pages.");

        for (int i = 0; i < count; i++)
        {
            // Instantiate PagePrefab
            GameObject lInstance = Instantiate(pagePrefab, book.LeftPageTransform);
            GameObject rInstance = Instantiate(interactionPagePrefab, book.RightPageTransform);

            // Get ContentPageUI component
            ContentPageUI lPageUI = lInstance.GetComponent<ContentPageUI>();
            PrologueInteractionPageUI rPageUI = rInstance.GetComponent<PrologueInteractionPageUI>();
            if (lPageUI == null || rPageUI == null)
            {
                Debug.LogError("PagePrefab must contain a ContentPageUI component.");
                continue;
            }

            // Assign the single face as the page front
            // No back side is used in your design.
            AddPaper(lPageUI.gameObject, rPageUI.gameObject, pages[i]);

            // Fill content (localized text, images, etc.)
            ApplyContentToPage(lPageUI, rPageUI, pages[i]);

        }

        //foreach (var paper in book.papers)
        //{
        //    Debug.Log(paper.Left.name + " | " + paper.Right.name);
        //}
    }

    // ======================================================================
    // FILL CONTENT INTO ONE PAGE USING ContentPageUI
    // ======================================================================
    private void ApplyContentToPage(ContentPageUI ContentPageUI, PrologueInteractionPageUI InteractionPageUI, PrologueJournalContentSO content)
    {
        // Localized journal body text
        if (ContentPageUI != null && ContentPageUI.videoPlayer != null)
        {
            if (content.LeftVideoClip != null)
            {
                ContentPageUI.videoPlayer.clip = content.LeftVideoClip;
            }
            else
            {
                Debug.LogWarning("Journal content is missing a video clip.");
            }

            ContentPageUI.EndImage.gameObject.SetActive(false);
            ContentPageUI.EndImage.sprite = content.LeftEnd;
        }
        if (InteractionPageUI != null)
        {
            if (InteractionPageUI.EndImage != null)
            {
                InteractionPageUI.EndImage.sprite = content.RightEnd;
            }
            if (InteractionPageUI.InteractionImage != null)
            {
                InteractionPageUI.InteractionImage.sprite = content.InteractionImageSO.StickerSprite;
            }
        }
    }

    // ======================================================================
    // PAGE NAVIGATION (Journal manager / input calls these)
    // ======================================================================

    private void Flip(bool isToLeft)
    {
        if (isToLeft)
        {
            book.FlipToLeft();
            PreviousPage();
        }
        else
        {
            book.FlipToRight();
            NextPage();
        }
    }
    public void NextPage()
    {
        if (book.CurrentPaper > book.EndFlippingPaper){
            // Reached last journal page
            _journalData.FinishJournalSection();
        }
    }

    public void PreviousPage()
    {
        //if (book.CurrentPaper > 0)
        //{
        //    book.CurrentPaper--;
        //}
    }

    public void ChangeBookPageAccess(int unlockNum)
    {
        book.EndFlippingPaper += unlockNum;
    }
}
