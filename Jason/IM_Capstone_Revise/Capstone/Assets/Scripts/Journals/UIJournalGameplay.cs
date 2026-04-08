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

    [SerializeField] private Sprite _defaultImg;

    private GameplayJournalDataSO _journalData;
    private GameplayJournalDataSO _parsedJournalData;

    private bool _rightPageEmpty = false;
    private GameplayContentPageUI _waitingRightPage = null;

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
        Debug.Log("Initializing journal UI with data: " + data.Pages.Count + " pages.");
        _journalData = data;

        ParseDataPassed();
        CreatePagesForBook();

    }

    void ParseDataPassed()
    {
        var rightPageEmpty = _rightPageEmpty; 
        GameplayJournalContentSO pendingLeftPage = null;
        _parsedJournalData = ScriptableObject.CreateInstance<GameplayJournalDataSO>();

        foreach (var page in _journalData.Pages)
        {
            if (page.RightIMG == null) // 只有左面的单页数据
            {
                if (rightPageEmpty && pendingLeftPage != null)
                {
                    // 将当前单页作为右页，与等待的左页合并成一个完整页面
                    pendingLeftPage.Initialize(
                        pendingLeftPage.LeftIMG,
                        pendingLeftPage.LeftText,
                        page.LeftIMG,
                        page.LeftText
                    );
                    _parsedJournalData.Pages.Add(pendingLeftPage);
                    rightPageEmpty = false;
                    pendingLeftPage = null;
                    Debug.Log("This is what is supposed toi happen");
                }
                else if(rightPageEmpty && pendingLeftPage == null)
                {
                    pendingLeftPage = ScriptableObject.CreateInstance<GameplayJournalContentSO>();
                    pendingLeftPage.Initialize(null, null, page.LeftIMG, page.LeftText);
                    rightPageEmpty = false;
                }
                else
                {
                    // 创建新的左页，等待右页
                    pendingLeftPage = ScriptableObject.CreateInstance<GameplayJournalContentSO>();
                    pendingLeftPage.Initialize(page.LeftIMG, page.LeftText, null, null);
                    rightPageEmpty = true;
                }
            }
            else // 完整双页（左右都有）
            {
                // 如果有等待配对的左页，先把它作为单独页面添加（只有左半部分）
                if (rightPageEmpty && pendingLeftPage != null)
                {
                    _parsedJournalData.Pages.Add(pendingLeftPage);
                    rightPageEmpty = false;
                    pendingLeftPage = null;
                }

                // 直接添加完整双页
                _parsedJournalData.Pages.Add(page);
            }
        }

        // 处理最后未配对的左页（作为只有左半部分的页面）
        if (pendingLeftPage != null)
        {
            _parsedJournalData.Pages.Add(pendingLeftPage);
        }
    }

    // ======================================================================
    // CREATE PAGE INSTANCES & ASSIGN INTO BookPro
    // ======================================================================

    public void AddPaper(GameObject LeftPage, GameObject RightPage)
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
        List<GameplayJournalContentSO> pages = _parsedJournalData.Pages;

        if (_rightPageEmpty)
        {
            Debug.Log("I was waiting for right page");
            _waitingRightPage.image.sprite = pages[0].RightIMG;
            pages.RemoveAt(0);
            _rightPageEmpty = false;
        }

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
            AddPaper(lPageUI.gameObject, rPageUI.gameObject);

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
        LPageUI.usedBackground.sprite = LPageUI.backgrounds[0];
        RPageUI.usedBackground.sprite = RPageUI.backgrounds[1];

        if (LPageUI.image != null && content.LeftIMG != null)
        {
            LPageUI.image.sprite = content.LeftIMG;
        }

        if (RPageUI.image != null)
        {
            if (content.RightIMG == null)
            {
                _rightPageEmpty = true;
                RPageUI.image.sprite = _defaultImg;
                _waitingRightPage = RPageUI;
            }
            else
            {
                RPageUI.image.sprite = content.RightIMG;
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
