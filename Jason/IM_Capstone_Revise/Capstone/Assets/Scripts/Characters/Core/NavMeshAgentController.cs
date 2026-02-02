using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Core component which wraps a <see cref="NavMeshAgent"/> and provides
/// high‑level control over its movement. This component exposes methods to
/// assign destinations, stop the agent and query when it has arrived. It
/// integrates with the existing <see cref="Movement"/> component so that
/// 2D physics and facing logic remain consistent with the navmesh driven
/// motion. When attached to a GameObject with a NavMeshAgent and a Core
/// hierarchy the agent will automatically register itself with the entity's
/// <see cref="Core"/> during Awake.
/// </summary>
public class NavMeshAgentController : CoreComponent
{
    /// <summary>
    /// Reference to the wrapped NavMeshAgent. Exposed for read‑only access
    /// should other systems need to query agent properties.
    /// </summary>
    public NavMeshAgent Agent = default;

    private Movement _movement;

    // Threshold used to determine when the agent is considered to have reached
    // its destination. This is compared against NavMeshAgent.remainingDistance.
    [SerializeField]
    private float arriveDistance = 0.1f;

    // Base speed configured in the inspector. If the NavMeshAgent exists this
    // value will be applied on Awake. Changing the Speed property at runtime
    // will override the agent's speed directly.
    [SerializeField]
    private float baseSpeed = 3.5f;

    protected override void Awake()
    {
        base.Awake();

        if (Agent == null)
        {
            Debug.LogError($"{nameof(NavMeshAgent)} not found on {transform.parent?.name ?? name}");
            return;
        }

        // Fetch the Movement component from the Core if available. It is not
        // required – when absent the controller will directly update the
        // GameObject transform.
        _movement = core.GetCoreComponent<Movement>();

        // For 2D games we do not want the NavMeshAgent to rotate the
        // transform or align it to the 3D up axis. Disabling these allows
        // the Movement component to fully own rotation/facing, and avoids
        // unwanted z‑movement. updatePosition is also disabled so that the
        // controller can apply the simulated position via Movement or direct
        // transform manipulation.
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        Agent.updatePosition = false;

        // Apply the configured base speed to the agent. Unity's documentation
        // notes that the speed value defines the maximum movement speed an
        // agent may reach while following a path【279273260033067†L103-L115】. Setting
        // it here allows designers to tweak speeds per entity via the
        // inspector.
        if (Agent != null)
        {
            Agent.speed = baseSpeed;
        }
    }

    /// <summary>
    /// Called each frame from the Core. Updates the Movement component or the
    /// transform based on the NavMeshAgent's simulated next position and
    /// velocity. This keeps the physics body and facing direction in sync
    /// with the navmesh simulation.
    /// </summary>
    public override void LogicUpdate()
    {
        if (Agent == null)
            return;

        // The NavMeshAgent internally simulates along the path even when
        // updatePosition is false. Use nextPosition to retrieve the current
        // simulated position and velocity to derive facing.
        Vector3 next = Agent.nextPosition;
        Vector3 velocity = Agent.velocity;

        if (_movement != null)
        {
            // Apply the navmesh position directly to the physics body to avoid
            // conflicting forces. ForceChangePosition resets velocity to zero
            // so we apply the velocity separately via SetVelocity.
            // Cast next position down to 2D when driving a Rigidbody2D.
            _movement.ForceChangePosition(new Vector2(next.x, next.y));
            _movement.SetVelocity(new Vector2(velocity.x, velocity.y));

            // Update facing angle/direction if the agent is moving.
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                _movement.FacingAngle = angle;
                _movement.FacingDirection = Movement.AngleToFacingDir(angle);
            }
        }
        else
        {
            // As a fallback when no Movement component is present, move
            // the transform directly. Preserve the existing z coordinate.
            transform.position = new Vector3(next.x, next.y, transform.position.z);
        }
    }

    /// <summary>
    /// Assigns a destination for the agent to move towards. This will
    /// immediately start path calculation and once complete the agent
    /// simulation will begin moving along the path. If the agent is
    /// currently stopped or has a cleared path, this will implicitly
    /// re‑enable movement.
    /// </summary>
    /// <param name="destination">The world position in 2D or 3D space to move towards.</param>
    public void SetDestination(Vector3 destination)
    {
        if (Agent == null)
            return;

        Agent.isStopped = false;
        Agent.SetDestination(destination);
    }

    /// <summary>
    /// Overload for 2D destinations to avoid needing to provide a z component.
    /// </summary>
    public void SetDestination(Vector2 destination)
    {
        SetDestination(new Vector3(destination.x, destination.y, 0f));
    }

    /// <summary>
    /// Immediately stops the agent and clears any existing path. After
    /// stopping the agent will remain idle until a new destination is set.
    /// </summary>
    public void Stop()
    {
        if (Agent == null)
            return;

        Agent.isStopped = true;
        Agent.ResetPath();
    }

    /// <summary>
    /// Returns true when the agent has reached its current destination within
    /// the arriveDistance threshold. This accounts for path calculation
    /// still pending and ensures movement has nearly ceased.
    /// </summary>
    public bool HasReachedDestination()
    {
        if (Agent == null)
            return true;
        if (Agent.pathPending)
            return false;
        return Agent.remainingDistance <= arriveDistance && (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f);
    }

    /// <summary>
    /// Configurable radius used by <see cref="HasReachedDestination"/> to
    /// determine arrival. A larger value allows the agent to consider itself
    /// arrived sooner, while a smaller value requires it to approach more
    /// closely to the destination point.
    /// </summary>
    public float ArriveDistance
    {
        get => arriveDistance;
        set => arriveDistance = Mathf.Max(0f, value);
    }

    /// <summary>
    /// Gets or sets the maximum movement speed of the wrapped NavMeshAgent. When
    /// assigned, this will directly set <see cref="NavMeshAgent.speed"/>, which
    /// defines the fastest speed the agent will travel along a path【279273260033067†L103-L115】.
    /// If the agent reference is missing this property will operate on the
    /// serialized <c>baseSpeed</c> instead.
    /// </summary>
    public float Speed
    {
        get => Agent != null ? Agent.speed : baseSpeed;
        set
        {
            baseSpeed = Mathf.Max(0f, value);
            if (Agent != null)
            {
                Agent.speed = baseSpeed;
            }
        }
    }

    /// <summary>
    /// Shortcut method to modify the agent's speed at runtime. Equivalent
    /// to setting the <see cref="Speed"/> property.
    /// </summary>
    /// <param name="newSpeed">New maximum speed for the agent in units/sec.</param>
    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }
}