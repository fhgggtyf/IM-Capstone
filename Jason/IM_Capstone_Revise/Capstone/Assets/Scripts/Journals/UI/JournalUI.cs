using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BookCurlPro;
using System;

public class JournalUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BookPro book;                // The BookPro flipping system
    [SerializeField] private GameObject pagePrefab;       // Prefab containing PageUI + "Front"
    [SerializeField] private GameObject JournalBCover;     // The back cover page
    [SerializeField] private AutoFlip _flip;

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
    public void Initialize(JournalDataSO data, bool isBack)
    {
        _journalData = data;

        CreatePagesForBook(isBack);

    }

    // ======================================================================
    // CREATE PAGE INSTANCES & ASSIGN INTO BookPro
    // ======================================================================

    public void AddPaper(GameObject frontPage, GameObject backPage)
    {
        Debug.Log("Adding a new paper to the book.");
        frontPage.transform.SetParent(book.transform, false);
        backPage.transform.SetParent(book.transform, false);
        Paper newPaper = new Paper();
        newPaper.Front = frontPage;
        newPaper.Back = backPage;
        Paper[] papers = new Paper[book.papers.Length + 1];
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
    private void CreatePagesForBook(bool isBack)
    {
        List<JournalContentSO> pages = _journalData.Pages;
        int count = pages.Count;
        Debug.Log($"Creating {count} journal pages.");

        for (int i = 0; i < count; i += 2)
        {
            // Instantiate PagePrefab
            GameObject finstance = Instantiate(pagePrefab, book.transform);
            GameObject binstance = Instantiate(pagePrefab, book.transform);

            // Get PageUI component
            PageUI fpageUI = finstance.GetComponent<PageUI>();
            PageUI bpageUI = binstance.GetComponent<PageUI>();
            if (fpageUI == null || bpageUI == null)
            {
                Debug.LogError("PagePrefab must contain a PageUI component.");
                continue;
            }

            // Assign the single face as the page front
            // No back side is used in your design.
            AddPaper(fpageUI.gameObject, bpageUI.gameObject);

            // Fill content (localized text, images, etc.)
            ApplyContentToPage(fpageUI, pages[i]);
            if (i + 1 < count)
                ApplyContentToPage(bpageUI, pages[i + 1]);
            else
                Debug.Log("Odd number of pages; leaving last page blank.");
        }


        if (isBack) { 
            GameObject BCoverObject = Instantiate(JournalBCover, book.transform);
            var bholder = BCoverObject.GetComponent<PaperHolder>();
            Debug.Log(bholder.front.name);
            AddPaper(bholder.front, bholder.back);
        }

        //foreach (var paper in book.papers)
        //{
        //    Debug.Log(paper.Front.name + " | " + paper.Back.name);
        //}
    }

    // ======================================================================
    // FILL CONTENT INTO ONE PAGE USING PageUI
    // ======================================================================
    private void ApplyContentToPage(PageUI ui, JournalContentSO content)
    {
        // Localized journal body text
        if (ui.pageText != null)
        {
            content.Text.GetLocalizedStringAsync().Completed += h =>
            {
                ui.pageText.text = h.Result;
            };
        }

        // Localized date
        if (ui.pageDate != null)
        {
            content.Date.GetLocalizedStringAsync().Completed += h =>
            {
                ui.pageDate.text = h.Result;
            };
        }

        // Localized time
        if (ui.pageTime != null)
        {
            content.Time.GetLocalizedStringAsync().Completed += h =>
            {
                ui.pageTime.text = h.Result;
            };
        }

        // Image (optional)
        if (ui.pageImage != null)
        {
            ui.pageImage.sprite = content.Image;
        }
    }

    // ======================================================================
    // PAGE NAVIGATION (Journal manager / input calls these)
    // ======================================================================

    private void Flip(bool isToLeft)
    {
        if (isToLeft)
        {
            _flip.FlipLeftPage();
            PreviousPage();
        }
        else
        {
            _flip.FlipRightPage();
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
        if (book.CurrentPaper > 0)
        {
            book.CurrentPaper--;
        }
    }

    public void ChangeBookPageAccess(int unlockNum)
    {
        book.EndFlippingPaper += unlockNum;
    }
}
