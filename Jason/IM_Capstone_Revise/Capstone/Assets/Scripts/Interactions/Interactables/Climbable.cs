using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Climbable : MonoBehaviour, IInteractable
{
    public ClimbTypes climbType;
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

    public void Interact()
    {

    }

    private void Awake()
    {
    }
}

public enum ClimbTypes
{
    Rope,
    Ladder
}
