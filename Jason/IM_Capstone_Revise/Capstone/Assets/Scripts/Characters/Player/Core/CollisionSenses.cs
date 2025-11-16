using UnityEngine;


public class CollisionSenses : CoreComponent
{

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    #region Check Transforms

    public Transform GroundCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
        private set => groundCheck = value;
    }
    public Transform ClimbableCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(climbableCheck, core.transform.parent.name);
        private set => climbableCheck = value;
    }
    public Transform WallCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
        private set => wallCheck = value;
    }
    public Transform OffClimbCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(offClimbCheck, core.transform.parent.name);
        private set => offClimbCheck = value;
    }
    public Transform CeilingCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, core.transform.parent.name);
        private set => ceilingCheck = value;
    }
    public Transform CrouchCeilingCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(crouchCeilingCheck, core.transform.parent.name);
        private set => crouchCeilingCheck = value;
    }
    public Transform LedgeCheckHorizontal
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
        private set => ledgeCheckHorizontal = value;
    }
    public Transform LedgeCheckVertical
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, core.transform.parent.name);
        private set => ledgeCheckVertical = value;
    }

    public float ClimbableCheckDistance { get => climbableCheckDistance; set => climbableCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform climbableCheck;
    [SerializeField] private Transform offClimbCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck; 
    [SerializeField] private Transform crouchCeilingCheck;

    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float offClimbCheckRadius;
    [SerializeField] private float climbableCheckDistance;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsClimbable;

    #endregion

    public bool Ceiling
    {
        get => Physics2D.OverlapArea(CeilingCheck.position - groundCheckOffset, CeilingCheck.position + groundCheckOffset, whatIsGround);
    }

    public bool CrouchCeiling
    {
        get => Physics2D.OverlapArea(CrouchCeilingCheck.position - groundCheckOffset, CrouchCeilingCheck.position + groundCheckOffset, whatIsGround);
    }

    public bool Ground
    {
        get => Physics2D.OverlapArea(GroundCheck.position - groundCheckOffset, GroundCheck.position + groundCheckOffset, whatIsGround);
    }
    public bool OffClimb
    {
        get => !Physics2D.OverlapCircle(offClimbCheck.position, offClimbCheckRadius, whatIsClimbable);
    }

    public bool ClimbableFront
    {
        get => Physics2D.Raycast(ClimbableCheck.position, Vector2.right * Movement.FacingDirection, climbableCheckDistance, whatIsClimbable);
    }

    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }

    public bool LedgeVertical
    {
        get => !Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }

}

