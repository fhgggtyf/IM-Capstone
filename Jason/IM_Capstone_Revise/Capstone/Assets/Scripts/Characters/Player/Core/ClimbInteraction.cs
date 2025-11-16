using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbInteraction : CoreComponent
{
    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    protected CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private InteractableDetector interactableDetector;
    private Climbable _climbable;

    [SerializeField] private Player _player;
    [SerializeField] private TransformEventChannelSO _toClimb;

    public Climbable Climbable { get => _climbable; set => _climbable = value; }

    private void HandleTryInteract(IInteractable interactable)
    {
        if (interactable is not Climbable climbable)
            return;

        _climbable = climbable;

        _player.isClimbing = true;

        _toClimb.RaiseEvent(_climbable.transform);

        if (!CollisionSenses.WallFront && _climbable.climbType == ClimbTypes.Rope)
        {
            Movement.Flip();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        interactableDetector = core.GetCoreComponent<InteractableDetector>();
    }

    private void OnEnable()
    {
        interactableDetector.OnTryInteract += HandleTryInteract;
    }


    private void OnDisable()
    {
        interactableDetector.OnTryInteract -= HandleTryInteract;
    }
}
