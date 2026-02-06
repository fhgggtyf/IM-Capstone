using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public FacingDir FacingDirection { get; set; }

    /// <summary>
    /// The continuous facing angle in degrees, derived from the entity's current
    /// movement direction. A value of 0 means facing right (positive X axis),
    /// 90 means facing up (positive Y), 180 means left and 270 means down.
    /// This angle is used for smooth rotations (e.g. vision cones) while
    /// <see cref="FacingDirection"/> provides a coarse 4‑way direction for
    /// animations and state machine conditions.
    /// </summary>
    public float FacingAngle { get; set; }

    public bool CanSetVelocity { get; set; }

    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();

        FacingDirection = FacingDir.Down;
        // Initialise FacingAngle to NaN to indicate that no movement-based
        // angle has been computed yet. This allows systems such as the
        // VisionCone to detect when the entity has not moved and fall back
        // to discrete facing logic. Without this, the angle defaults to
        // zero degrees (right), which may cause incorrect orientation for
        // stationary enemies.
        FacingAngle = 270f;
        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    #region Set Functions

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocity(Vector2 final)
    {
        workspace = final;
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        Debug.Log("Setting velocity to: " + workspace);
        if (CanSetVelocity)
        {
            RB.velocity = workspace;
            CurrentVelocity = workspace;

            switch (CurrentVelocity)
            {
                case Vector2 v when v.x > 0:
                    FacingDirection = FacingDir.Right;
                    break;
                case Vector2 v when v.x < 0:
                    FacingDirection = FacingDir.Left;
                    break;
                case Vector2 v when v.y > 0:
                    FacingDirection = FacingDir.Up;
                    break;
                case Vector2 v when v.y < 0:
                    FacingDirection = FacingDir.Down;
                    break;
                default:
                    break;
            }
        }
    }

    public void ForceChangePositionX(Transform transform)
    {
        RB.transform.position = new Vector2(transform.position.x, RB.transform.position.y);
        RB.velocity = new Vector2(0, 0);
    }
    public void ForceChangePositionY(Transform transform)
    {
        RB.transform.position = new Vector2(RB.transform.position.x, transform.position.y);
        RB.velocity = new Vector2(0, 0);
    }

    public void ForceChangePositionZ(float Zval)
    {
        RB.transform.position = new Vector3(RB.transform.position.x, RB.transform.position.y, Zval);
        RB.velocity = new Vector2(0, 0);
    }
    public void ForceChangePosition(Transform transform)
    {
        RB.transform.position = new Vector2(transform.position.x, transform.position.y);
        RB.velocity = new Vector2(0, 0);
    }
    public void ForceChangePosition(Vector2 vec)
    {
        RB.transform.position = vec;
        RB.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// Converts a continuous angle in degrees into one of the four
    /// <see cref="FacingDir"/> values. Angles are normalised to [0,360).
    /// The resulting ranges are:
    ///  - Right:   [315°,45°)
    ///  - Up:      [45°,135°)
    ///  - Left:    [135°,225°)
    ///  - Down:    [225°,315°)
    /// </summary>
    /// <param name="angle">Angle in degrees.</param>
    /// <returns>Discrete facing direction derived from the angle.</returns>
    public static FacingDir AngleToFacingDir(float angle)
    {
        angle = Mathf.Repeat(angle, 360f);
        if (angle >= 45f && angle < 135f)
            return FacingDir.Up;
        if (angle >= 135f && angle < 225f)
            return FacingDir.Left;
        if (angle >= 225f && angle < 315f)
            return FacingDir.Down;
        return FacingDir.Right;
    }


    #endregion
}

public enum FacingDir
{
    Up, Down, Left, Right,
}

