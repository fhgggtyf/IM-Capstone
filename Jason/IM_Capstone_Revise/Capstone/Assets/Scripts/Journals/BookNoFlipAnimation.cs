using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

/// <summary>
/// A simplified "book" UI controller that ONLY tracks which paper is currently shown.
///
/// All page-curl / dragging / tweening / clipping-plane logic has been removed.
/// Switching pages is an instant, one-frame change via UpdatePages().
/// </summary>
public class BookNoFlipAnimation : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private RectTransform BookPanel;
    [SerializeField] public RectTransform LeftPageTransform;
    [SerializeField] public RectTransform RightPageTransform;


    [SerializeField] public InputReader inputReader;

    [SerializeField] public VoidEventChannelSO JournalEndedEvent;

    [Header("Behaviour")]
    public bool interactable = true;

    [Tooltip("Uncheck this if the book does not contain transparent pages to improve overall performance")]
    public bool hasTransparentPages = true;

    [Header("Pages")]
    public int currentPaper = 0;
    public Face[] papers;

    /// <summary>
    /// Called whenever the shown pages change (Next/Prev/Jump).
    /// </summary>
    public UnityEvent OnFlip;

    [HideInInspector] public int StartFlippingPaper = 0;
    [HideInInspector] public int EndFlippingPaper = 1;

    /// <summary>
    /// The current shown paper index (the paper whose FRONT is shown on the right side).
    /// Setting this updates the visible pages immediately.
    /// </summary>
    public int CurrentPaper
    {
        get => currentPaper;
        set
        {
            int clamped = ClampPaperIndex(value);
            if (clamped != currentPaper)
            {
                currentPaper = clamped;
                UpdatePages();
                OnFlip?.Invoke();
            }
        }
    }

    private void Awake()
    {
        // Keep EndFlippingPaper consistent with the configured papers.
        if (papers != null && papers.Length > 0)
        {
            EndFlippingPaper = papers.Length - 1;
            currentPaper = ClampPaperIndex(currentPaper);
        }
    }

    private void Start()
    {
        // If you previously relied on calling Initialize() manually, you still can.
        // We also do a safe initial refresh here.
        Initialize();
    }

    /// <summary>
    /// Call after assigning papers at runtime, or if you want to re-sync everything.
    /// </summary>
    public void Initialize()
    {
        if (papers == null || papers.Length == 0)
        {
            EndFlippingPaper = 0;
            return;
        }

        EndFlippingPaper = papers.Length - 1;
        currentPaper = ClampPaperIndex(currentPaper);
        UpdatePages();
    }

    /// <summary>
    /// Instant next page (Right-To-Left in the old animation system).
    /// </summary>
    public void NextPage()
    {
        if (papers == null || papers.Length == 0) return;
        if (currentPaper >= EndFlippingPaper)
        {
            JournalEndedEvent.RaiseEvent();
            return;
        }

        CurrentPaper = currentPaper + 1;
    }

    /// <summary>
    /// Instant previous page (Left-To-Right in the old animation system).
    /// </summary>
    public void PreviousPage()
    {
        if (papers == null || papers.Length == 0) return;
        if (currentPaper <= StartFlippingPaper) return;

        CurrentPaper = currentPaper - 1;
    }

    /// <summary>
    /// Jump directly to a paper index.
    /// </summary>
    public void GoToPaper(int paperIndex)
    {
        if (papers == null || papers.Length == 0) return;

        CurrentPaper = paperIndex;
    }

    /// <summary>
    /// Update page visibility and parent/sibling order.
    /// This is the core "page tracking" logic kept from the original script.
    /// </summary>
    public void UpdatePages()
    {
        if (papers == null || papers.Length <= 1)
            return;

        if (currentPaper >= papers.Length)
            return;

        if(papers[currentPaper].IsInteractable)
        {
            inputReader.DisableAllInput();
            papers[currentPaper].IsInteractable = false;
        }

        int previousPaper = currentPaper - 1;

        // Hide all pages first.
        for (int i = 0; i < papers.Length; i++)
        {
            BookUtility.HidePage(papers[i].Left);
            papers[i].Left.transform.SetParent(LeftPageTransform);

            BookUtility.HidePage(papers[i].Right);
            papers[i].Right.transform.SetParent(RightPageTransform); 
        }

        BookUtility.ShowPage(papers[currentPaper].Left);
        BookUtility.CopyTransform(LeftPageTransform.transform, papers[currentPaper].Left.transform);
        BookUtility.ShowPage(papers[currentPaper].Right);
        BookUtility.CopyTransform(RightPageTransform.transform, papers[currentPaper].Right.transform);

    }

    private int ClampPaperIndex(int value)
    {
        if (papers == null || papers.Length == 0)
            return 0;

        int min = StartFlippingPaper;
        int max = EndFlippingPaper + 1; // matches the original clamp behaviour

        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>
    /// Flip the book visually toward the LEFT (the right-hand page advances).
    /// Typical "Next" behavior in a left-to-right book.
    /// </summary>
    public void FlipToLeft()
    {
        // Same as NextPage(): advances currentPaper by +1
        NextPage();
    }

    /// <summary>
    /// Flip the book visually toward the RIGHT (the left-hand page goes back).
    /// Typical "Previous" behavior in a left-to-right book.
    /// </summary>
    public void FlipToRight()
    {
        // Same as PreviousPage(): decreases currentPaper by -1
        PreviousPage();
    }
}

[Serializable]
public class Face
{
    public GameObject Left;
    public GameObject Right;
    public bool IsInteractable;
}

public static class BookUtility
{
    /// <summary>
    /// Show a hidden page (expects a CanvasGroup on the page GameObject).
    /// </summary>
    public static void ShowPage(GameObject page)
    {
        CanvasGroup cgf = page.GetComponent<CanvasGroup>();
        cgf.alpha = 1;
        cgf.blocksRaycasts = true;
    }

    /// <summary>
    /// Hide a page (expects a CanvasGroup on the page GameObject).
    /// </summary>
    public static void HidePage(GameObject page)
    {
        CanvasGroup cgf = page.GetComponent<CanvasGroup>();
        cgf.alpha = 0;
        cgf.blocksRaycasts = false;
        page.transform.SetAsFirstSibling();
    }

    public static void CopyTransform(Transform from, Transform to)
    {
        to.position = from.position;
        to.rotation = from.rotation;
        to.localScale = from.localScale;
    }
}
