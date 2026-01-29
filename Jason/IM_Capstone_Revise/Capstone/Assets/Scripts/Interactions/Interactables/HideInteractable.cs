using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HideInteractable : InteractableItems
{
    [SerializeField] private GameObject _hideIndicator;

    private bool _isHidden = false;

    public override void Interact()
    {
        if (_isHidden)
        {
            ExitHiding();
            _isHidden = false;
        }
        else
        {
            EnterHiding();
            _isHidden = true;
        }
    }

    private void EnterHiding()
    {
        _hideIndicator.SetActive(true);
    }

    private void ExitHiding()
    {
        _hideIndicator.SetActive(false);
    }
}

