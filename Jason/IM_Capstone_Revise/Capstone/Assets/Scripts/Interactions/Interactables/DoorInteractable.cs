using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DoorInteractable : InteractableItems
{
    [SerializeField] private GameObject _doorOpen;
    [SerializeField] private GameObject _doorClose;
    [SerializeField] private Collider2D _doorCollider;

    private bool _doorIsOpened = false;

    public override void Interact()
    {
        if (_doorIsOpened)
        {
            CloseDoor();
            _doorIsOpened = false;
        }
        else
        {
            OpenDoor();
            _doorIsOpened = true;
        }
    }

    private void OpenDoor()
    {
        _doorOpen.SetActive(true);
        _doorClose.SetActive(false);
        //_doorCollider.isTrigger = true;
    }

    private void CloseDoor()
    {
        _doorOpen.SetActive(false);
        _doorClose.SetActive(true);
        //_doorCollider.isTrigger = false;
    }
}

