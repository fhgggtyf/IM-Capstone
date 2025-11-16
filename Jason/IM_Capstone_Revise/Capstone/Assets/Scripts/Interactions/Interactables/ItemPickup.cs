using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemPickup : MonoBehaviour, IInteractable<ItemSO>
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [SerializeField] private SpriteRenderer Icon;
    [SerializeField] private Bobber bobber;

    [SerializeField] private ItemStack ItemDataStack;

    private ItemSO ItemData;

    public ItemSO GetContext() => ItemData;
    public int GetItemCount() => ItemDataStack.Amount;

    public void SetContext(ItemSO context)
    {
        ItemData = context;

        Icon.sprite = ItemData.Icon;
    }

    public void EnableInteraction()
    {
        bobber.StartBobbing();
    }

    public void DisableInteraction()
    {
        bobber.StopBobbing();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        Rigidbody2D ??= GetComponent<Rigidbody2D>();
        Icon ??= GetComponentInChildren<SpriteRenderer>();

        ItemData = ItemDataStack.Item;

        if (ItemData is null)
            return;

        Icon.sprite = ItemData.Icon;
    }
}
