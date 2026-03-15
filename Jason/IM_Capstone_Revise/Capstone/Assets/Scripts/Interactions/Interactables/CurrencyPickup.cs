using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CurrencyPickup : MonoBehaviour, IInteractable<CurrencyDataSO>
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [SerializeField] private SpriteRenderer currencyIcon;
    [SerializeField] private OutlineController bobber;

    [SerializeField] private CurrencyDataSO currencyData;

    public CurrencyDataSO GetContext() => currencyData;

    public void SetContext(CurrencyDataSO context)
    {
        currencyData = context;

        currencyIcon.sprite = currencyData.Icon;
    }

    public void EnableInteraction()
    {
        bobber.EnableOutline();
    }

    public void DisableInteraction()
    {
        bobber.DisableOutline();
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
        currencyIcon ??= GetComponentInChildren<SpriteRenderer>();

        if (currencyData is null)
            return;

        currencyIcon.sprite = currencyData.Icon;
    }
}
