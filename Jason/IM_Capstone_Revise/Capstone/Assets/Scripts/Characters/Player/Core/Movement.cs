using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; set; }

    public bool CanSetVelocity { get; set; }

    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField] private TransformEventChannelSO _toClimb;

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();

        FacingDirection = 1;
        CanSetVelocity = true;
        _toClimb.OnEventRaised += ForceChangePositionX;
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
        if (CanSetVelocity)
        {
            RB.velocity = workspace;
            CurrentVelocity = workspace;
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

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public Vector2 FindRelativePoint(Vector2 offset)
    {
        offset.x *= FacingDirection;

        return transform.position + (Vector3)offset;
    }

    #endregion
}

