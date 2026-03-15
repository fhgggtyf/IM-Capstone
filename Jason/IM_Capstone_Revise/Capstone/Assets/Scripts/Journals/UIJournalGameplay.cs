using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJournalGameplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BookNoFlipAnimation book;                // The BookPro flipping system
    [SerializeField] private GameObject leftPagePrefab;
    [SerializeField] private GameObject rightPagePrefab;

    [Header("Listening On")]
    [SerializeField] BoolEventChannelSO FlipToLeft;
    [SerializeField] IntEventChannelSO UnlockPages;

    private GameplayJournalDataSO _journalData;

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
    public void Initialize(GameplayJournalDataSO data)
    {
        _journalData = data;

        CreatePagesForBook();

    }

    // ======================================================================
    // CREATE PAGE INSTANCES & ASSIGN INTO BookPro
    // ======================================================================

    public void AddPaper(GameObject LeftPage, GameObject RightPage, GameplayJournalContentSO journalContent)
    {
        Debug.Log("Adding a new paper to the book.");
        LeftPage.transform.SetParent(book.LeftPageTransform, false);
        RightPage.transform.SetParent(book.RightPageTransform, false);
        Face newPaper = new Face();
        newPaper.Left = LeftPage;
        newPaper.Right = RightPage;
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
        List<GameplayJournalContentSO> pages = _journalData.Pages;
        int count = pages.Count;
        Debug.Log($"Creating {count} journal pages.");

        for (int i = 0; i < count; i++)
        {
            // Instantiate PagePrefab
            GameObject lInstance = Instantiate(leftPagePrefab, book.LeftPageTransform);
            GameObject rInstance = Instantiate(rightPagePrefab, book.RightPageTransform);

            // Get ContentPageUI component
            GameplayContentPageUI lPageUI = lInstance.GetComponent<GameplayContentPageUI>();
            GameplayContentPageUI rPageUI = rInstance.GetComponent<GameplayContentPageUI>();
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
    private void ApplyContentToPage(GameplayContentPageUI LPageUI, GameplayContentPageUI RPageUI, GameplayJournalContentSO content)
    {
        if (LPageUI.image != null)
        {
            LPageUI.image.sprite = content.LeftIMG.sprite;
        }

        if (RPageUI.image != null)
        {
            RPageUI.image.sprite = content.RightIMG.sprite;
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
        if (book.CurrentPaper > book.EndFlippingPaper)
        {
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
