using UnityEngine;
using Domicile.StateMachine;
using Domicile.StateMachine.ScriptableObjects;

/// <summary>
/// State action that updates an enemy's facing direction and continuous
/// facing angle based on its current movement velocity. This action
/// should be placed in any state where the enemy is allowed to move so
/// that the vision cone (and any other systems depending on facing
/// orientation) rotates to match the actual movement direction. The
/// movement component stores both a discrete <see cref="FacingDir"/> for
/// animations and a continuous <see cref="Movement.FacingAngle"/> in
/// degrees for smooth rotations. When no significant movement is
/// detected the action does nothing, preserving the last orientation.
/// </summary>
[CreateAssetMenu(
    fileName = "UpdateFacingDirectionAction",
    menuName = "State Machines/Actions/Enemies/Update Facing Direction")]
public class UpdateFacingDirectionActionSO : StateActionSO<UpdateFacingDirectionAction>
{
    /// <summary>
    /// Squared magnitude threshold below which velocity is considered
    /// negligible. If the enemy moves slower than this threshold the
    /// facing direction will not be updated. Increase this if subtle
    /// jitter in velocity should be ignored.
    /// </summary>
    [Tooltip("Squared magnitude threshold below which velocity is ignored when updating facing.")]
    public float minimumVelocitySqr = 0.0001f;
}

public class UpdateFacingDirectionAction : StateAction
{
    private Movement _movement;
    private UpdateFacingDirectionActionSO _origin;

    public override void Awake(StateMachine stateMachine)
    {
        var npc = stateMachine.GetComponent<NonPlayerCharacter>();
        if (npc != null && npc.Core != null)
        {
            _movement = npc.Core.GetCoreComponent<Movement>();
        }
        _origin = (UpdateFacingDirectionActionSO)OriginSO;
    }

    public override void OnUpdate()
    {
        if (_movement == null)
            return;

        // Obtain current velocity from the movement component. If there is no
        // significant movement we do not change the facing orientation.
        Vector2 velocity = _movement.CurrentVelocity;
        if (velocity.sqrMagnitude < _origin.minimumVelocitySqr)
            return;

        // Compute the angle in degrees between the positive X axis and the
        // velocity vector. Mathf.Atan2 returns radians; convert to degrees.
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // Persist the continuous facing angle for systems that need smooth
        // rotation, such as the VisionCone. Assigning directly updates the
        // property on the Movement component.
        _movement.FacingAngle = angle;

        // Also update the discrete four-way facing direction for use by
        // animations or other coarse directional logic. Use the helper
        // method on Movement to categorise the angle into a FacingDir.
        _movement.FacingDirection = Movement.AngleToFacingDir(angle);
    }
}