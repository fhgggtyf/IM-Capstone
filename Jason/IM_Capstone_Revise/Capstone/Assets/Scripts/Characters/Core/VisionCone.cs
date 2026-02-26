using UnityEngine;

/// <summary>
/// Logic-only vision cone. Computes IsPlayerInSight each frame.
/// Includes optional Gizmos visualization (Scene view).
/// </summary>
public class VisionCone : CoreComponent
{
    [Header("Target")]
    [Tooltip("If set, VisionCone will read the player transform from this anchor (recommended for spawned players).")]
    [SerializeField] private TransformAnchor playerTransformAnchor;
    [Tooltip("Optional override. If set, this is used instead of the anchor.")]
    public Transform playerTransform;

    [SerializeField] private BoolEventChannelSO playerIsHiding;

    [Header("Debug (Editor)")]
    public bool drawGizmos = true;

    private NonPlayerCharacter _npc;
    private NonPlayerStatsManager _stats;
    private Movement _movement;
    
    private bool playerHiding = false;

    public bool IsPlayerInSight { get; private set; }

    // ---- Public getters for a runtime visualizer (mesh/sprite) to read from ----
    public Vector2 GetOriginWorld() => transform.position;
    public Vector2 GetForwardWorld() => GetFacingVector();
    public float GetRange() => _stats != null ? _stats.GetVisionRange() : 0f;
    public float GetAngleDeg() => _stats != null ? _stats.GetVisionAngle() : 0f;

    protected override void Awake()
    {
        base.Awake();

        _npc = GetComponentInParent<NonPlayerCharacter>();
        _stats = GetComponentInParent<NonPlayerStatsManager>();

        if (_npc != null && _npc.Core != null)
            _movement = _npc.Core.GetCoreComponent<Movement>();

        playerIsHiding.OnEventRaised += i =>
        {
            // When the player hides, they should immediately be considered out of sight.
            if (i) {
                IsPlayerInSight = !i;
                if (_npc != null)
                {
                    _npc.playerIsInSight = !i;
                }
            }
            playerHiding = i;
        };
    }

    private void LateUpdate()
    {
        // Anchor hookup (preferred)
        if (playerTransform == null && playerTransformAnchor != null)
        {
            // Adjust this line if your TransformAnchor uses a different property name.
            // Common patterns are: playerTransformAnchor.Value or playerTransformAnchor.Transform
            playerTransform = playerTransformAnchor.Value;
        }

        IsPlayerInSight = ComputeIsPlayerInSight();
        _npc.playerIsInSight = IsPlayerInSight;
    }

    private bool ComputeIsPlayerInSight()
    {
        if (_npc == null || _stats == null || _movement == null || playerTransform == null || playerHiding)
            return false;

        Vector2 npcPos = _npc.transform.position;
        Vector2 playerPos = playerTransform.position;
        Vector2 toPlayer = playerPos - npcPos;

        float range = _stats.GetVisionRange();
        if (range <= 0f) return false;

        float dist = toPlayer.magnitude;
        if (dist > range)
            return false;

        float angle = _stats.GetVisionAngle();
        if (angle <= 0f) return false;

        Vector2 facing = GetFacingVector();
        float half = angle * 0.5f;

        float angleToPlayer = Vector2.Angle(facing, toPlayer);
        if (angleToPlayer > half)
            return false;

        Vector2 dir = toPlayer.normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(npcPos, dir, dist);
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.isTrigger) continue;

            // Ignore player's own collider(s)
            if (hit.collider.transform == playerTransform ||
                hit.collider.transform.IsChildOf(playerTransform))
                continue;

            HideInteractable hide = hit.collider.GetComponent<HideInteractable>();
            if (hide != null && hide.CanHideBehind)
            {
                // Vision is blocked by a hide-behind object
                return false;
            }
        }

        // No blockers ¡ú player visible
        return true;
    }


    private Vector2 GetFacingVector()
    {
        // Fallback if Movement is missing
        if (_movement == null)
        {
            return Vector2.up;
        }

        // Use the continuous facing angle from the Movement component for
        // determining the forward vector. This allows the vision cone to
        // smoothly rotate based on the actual movement direction rather
        // than snapping to the four cardinal directions. FacingAngle
        // represents the rotation in degrees with 0 pointing along +X.
        float angle = _movement.FacingAngle;

        // If no meaningful angle has been set yet (e.g., the enemy hasn't
        // moved or the angle has not been initialised), fall back to the
        // discrete facing direction. This ensures the vision cone still
        // works when standing still or before any movement has been
        // processed. We treat NaN as an indicator that no angle has been
        // calculated yet.
        if (float.IsNaN(angle))
        {
            switch (_movement.FacingDirection)
            {
                case FacingDir.Up:
                    return Vector2.up;
                case FacingDir.Down:
                    return Vector2.down;
                case FacingDir.Left:
                    return Vector2.left;
                case FacingDir.Right:
                    return Vector2.right;
                default:
                    return Vector2.up;
            }
        }

        // Convert degrees to radians and compute the unit vector. Use
        // Mathf functions so this compiles properly in Unity. Normalize
        // to safeguard against any floating-point imprecision.
        float radians = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        return dir.normalized;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        // We cant rely on _stats in edit-mode always being initialized,
        // but we can try to fetch it safely.
        var stats = _stats != null ? _stats : GetComponentInParent<NonPlayerStatsManager>();
        var movement = _movement != null ? _movement : GetComponentInParent<Movement>();

        float range = stats != null ? stats.GetVisionRange() : 0f;
        float angle = stats != null ? stats.GetVisionAngle() : 0f;
        if (range <= 0f || angle <= 0f) return;

        // Derive the forward vector using the movement component's continuous
        // facing angle if available. This ensures the editor gizmo matches
        // the runtime orientation used by the vision logic. If no angle
        // has been computed yet (e.g. before any movement), fall back to
        // the discrete FacingDirection values.
        Vector2 forward = Vector2.up;
        if (movement != null)
        {
            float angleDeg = movement.FacingAngle;
            if (!float.IsNaN(angleDeg))
            {
                float rad = angleDeg * Mathf.Deg2Rad;
                forward = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            }
            else
            {
                switch (movement.FacingDirection)
                {
                    case FacingDir.Up:
                        forward = Vector2.up;
                        break;
                    case FacingDir.Down:
                        forward = Vector2.down;
                        break;
                    case FacingDir.Left:
                        forward = Vector2.left;
                        break;
                    case FacingDir.Right:
                        forward = Vector2.right;
                        break;
                }
            }
        }

        Vector3 origin = transform.position;
        float half = angle * 0.5f;

        Vector2 leftDir = (Vector2)(Quaternion.Euler(0, 0, +half) * forward);
        Vector2 rightDir = (Vector2)(Quaternion.Euler(0, 0, -half) * forward);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + (Vector3)(leftDir * range));
        Gizmos.DrawLine(origin, origin + (Vector3)(rightDir * range));
        Gizmos.DrawWireSphere(origin, range);
    }
#endif
}
