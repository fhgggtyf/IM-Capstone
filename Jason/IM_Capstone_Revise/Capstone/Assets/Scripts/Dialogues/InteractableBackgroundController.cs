using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableBackgroundController : MonoBehaviour
{
    public List<InteractiveItemDataSO> InteractiveItems;
    public Image BackGround;
    public GameObject DialogueItemPrefab;
    public GameObject JournalItemPrefab;

    List<GameObject> itemGO = new();

    [SerializeField] GameObject interactiveRoot;

    public void InitializePanel(List<InteractiveItemDataSO> interactiveItem = null, Sprite bgImage = null)
    {
        interactiveRoot.SetActive(false);

        if (bgImage != null)
        {
            BackGround.gameObject.SetActive(true);
            BackGround.sprite = bgImage;
        }
        else
        {
            BackGround.gameObject.SetActive(false);
            BackGround.sprite = bgImage;
        }

        if (interactiveItem != null)
        {
            InteractiveItems = interactiveItem;
            PlaceItems();
        }
    }

    public void PlaceItems()
    {
        foreach (var item in InteractiveItems)
        {
            Debug.Log("Placing item: " + item.name);
            if (item.dialogue == null)
            {
                itemGO.Add(Instantiate(JournalItemPrefab, interactiveRoot.transform));
            }
            else
            {
                itemGO.Add(Instantiate(DialogueItemPrefab, interactiveRoot.transform));
            }
            itemGO[^1].GetComponent<InteractiveItem>().InteractiveItemDataSO = item;
        }
        interactiveRoot.SetActive(true);
    }

    public void ClearItems()
    {
        BackGround.gameObject.SetActive(false);
        BackGround.sprite = null;
        foreach (var item in itemGO)
        {
            Destroy(item);
        }
        itemGO.Clear();
        interactiveRoot.SetActive(false);
    }

}
