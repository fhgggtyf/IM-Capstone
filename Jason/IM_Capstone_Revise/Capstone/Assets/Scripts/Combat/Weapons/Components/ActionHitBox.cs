using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
{
    public event Action<Collider2D[]> OnDetectedCollider2D;

    private CoreComp<Movement> movement;

    private Vector2 offset;

    private Collider2D[] detected;
    private HashSet<Collider2D> _hasCounted;

    private bool _detecting;

    private void HandleAttackAction()
    {
        offset.Set(
            transform.position.x + (currentAttackData.HitBox.center.x * movement.Comp.FacingDirection),
            transform.position.y + currentAttackData.HitBox.center.y
        );

        detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);

        if (detected.Length == 0)
            return;

        Collider2D[] newCounted = detected.Where(collider => !_hasCounted.Contains(collider)).ToArray();

        OnDetectedCollider2D?.Invoke(newCounted);

        foreach (Collider2D coll in newCounted)
        {
            _hasCounted.Add(coll);
        }
    }

    protected override void Start()
    {
        base.Start();

        _hasCounted = new HashSet<Collider2D>();
        _detecting = false;
        movement = new CoreComp<Movement>(Core);

        AnimationEventHandler.OnAttackHitboxActive += IsDetectingEnemies;
        AnimationEventHandler.OnEnterAttackPhase += IsAttackStarted;
    }

    private void IsDetectingEnemies(bool param) => _detecting = param;
    private void IsAttackStarted(AttackPhases phase) => _hasCounted = new HashSet<Collider2D>();

    protected void Update()
    {
        if (_detecting)
        {
            HandleAttackAction();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AnimationEventHandler.OnAttackHitboxActive -= IsDetectingEnemies;
    }

    private void OnDrawGizmosSelected()
    {
        if (data == null)
            return;

        foreach (var item in data.GetAllAttackData())
        {
            if (!item.Debug)
                continue;

            Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
        }
    }
}
