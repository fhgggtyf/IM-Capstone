using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueRightSideInteractionController : MonoBehaviour
{
    public List<PrologueInteractionPageUI> interactionPages;
    public InputReader inputReader;

    [SerializeField] public VoidEventChannelSO ImageStampedEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (PrologueInteractionPageUI page in interactionPages)
        {
            if (page.gameObject.GetComponent<CanvasGroup>().alpha == 1)
            {
                page.InteractionButton.interactable = true;
                page.InteractionButton.onClick.RemoveAllListeners();
                page.InteractionButton.onClick.AddListener(() => {
                    page.InteractionImage.gameObject.SetActive(true);
                    page.InteractionButton.gameObject.SetActive(false);
                    inputReader.EnableJournalInput();
                    ImageStampedEvent.RaiseEvent();
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
