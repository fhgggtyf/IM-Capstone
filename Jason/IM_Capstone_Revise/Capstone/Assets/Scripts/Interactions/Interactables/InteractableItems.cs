using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour, IInteractable
{
    public virtual void DisableInteraction()
    {
    }

    public virtual void EnableInteraction()
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
