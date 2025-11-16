using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour, IInteractable
{
    public void DisableInteraction()
    {
    }

    public void EnableInteraction()
    {
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual void Interact()
    {

    }

}
