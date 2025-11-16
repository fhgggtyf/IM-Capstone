using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponMovement : WeaponComponent<WeaponMovementData, AttackMovement>
{

    private CoreComp<Movement> coreMovement;

    private float velocity;
    private Vector2 direction;
    private bool hasGravity;

    private float _timer;

    private void HandleStartMovement()
    {
        velocity = currentAttackData.Velocity;
        direction = currentAttackData.Direction;
        hasGravity = currentAttackData.HasGravity;
        _timer = 0;

        SetVelocity();
    }

    private void HandleStopMovement()
    {
        velocity = 0f;
        direction = Vector2.zero;

        if (!hasGravity)
        {
            SetVelocity();
        }

        hasGravity = false;
    }

    protected override void HandleEnter()
    {
        base.HandleEnter();

        velocity = 0f;
        direction = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!isAttackActive)
            return;

        SetVelocityX();
    }

    private void Update()
    {
        if (!isAttackActive)
            return;

        if (hasGravity)
        {
            Debug.Log(hasGravity);
            _timer += Time.deltaTime;
            SetVelocityY();
        }

    }

    private void SetVelocity()
    {
        coreMovement.Comp.SetVelocity(velocity, direction, coreMovement.Comp.FacingDirection);
    }

    private void SetVelocityY()
    {
        coreMovement.Comp.SetVelocityY(velocity * direction.y + _timer * -9.8f * GenericPhysicsData.GRAVITY_MULTIPLIER * (coreMovement.Comp.CurrentVelocity.y >= 0 ? 1 : 2));
    }

    private void SetVelocityX()
    {
        coreMovement.Comp.SetVelocityX((direction * velocity).x * coreMovement.Comp.FacingDirection);
    }

    protected override void Start()
    {
        base.Start();

        coreMovement = new CoreComp<Movement>(Core);

        AnimationEventHandler.OnStartMovement += HandleStartMovement;
        AnimationEventHandler.OnStopMovement += HandleStopMovement;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        AnimationEventHandler.OnStartMovement -= HandleStartMovement;
        AnimationEventHandler.OnStopMovement -= HandleStopMovement;
    }

}
