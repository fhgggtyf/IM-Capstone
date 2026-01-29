using System;
using UnityEngine;

/// <summary>
/// Renders a filled circle (disc) using a procedurally generated mesh, and updates its radius
/// when a noise-changed event is raised by a non-MonoBehaviour stats manager.
/// Built-in + 2D safe:
/// - Uses XY plane (useXZPlane = false)
/// - Forces a material (MeshRenderer won't show without one)
/// - Fixes triangle winding to avoid backface culling invisibility
/// - Uses Z offset for reliable "draw on top of sprites" behavior in 2D
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NoiseRadiusVisualization : CoreComponent
{
    [Header("Listening To")]
    [SerializeField] private IntEventChannelSO onNoiseRadiusChanged = default;

    [Header("Circle Mesh")]
    [Tooltip("How smooth the circle is. 64 is usually plenty.")]
    [Range(8, 256)]
    public int segments = 64;

    [Tooltip("If true, the disc lies flat on XZ (top-down 3D). If false, it lies on XY (2D).")]
    public bool useXZPlane = false; // IMPORTANT for Unity 2D (XY)

    [Header("Placement")]
    [Tooltip("Optional transform to follow (e.g., player). If null, uses this transform.")]
    public Transform followTarget;

    [Tooltip("Offset to avoid z-fighting with sprites/ground.")]
    public float planeOffset = 0.02f;

    [Tooltip("In 2D built-in, Z is the most reliable way to control draw order vs sprites.")]
    public float zOffset2D = -2f;

    [Header("Runtime")]
    [Tooltip("If your stats emit 'radius' already in world units, keep this 1.0.")]
    public float radiusMultiplier = 1f;

    [Tooltip("Hide the visualization when radius is <= this value.")]
    public float hideAtOrBelow = 0.001f;

    [Header("Rendering (Required)")]
    [Tooltip("Assign a Built-in shader material (recommended: Unlit/Transparent).")]
    public Material material;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    // We generate a unit disc mesh (radius = 1) and scale transform to match the desired radius.
    private float _currentRadius = 0f;

    protected override void Awake()
    {
        base.Awake();

        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        // Build unit disc once (IMPORTANT: uses fixed triangle winding for visibility)
        _meshFilter.sharedMesh = BuildUnitDiscMesh(segments, useXZPlane);

        // Force a material (without this, MeshRenderer can be invisible if inspector is None)
        if (material != null)
            _meshRenderer.sharedMaterial = material;
        else
            Debug.LogWarning($"{nameof(NoiseRadiusVisualization)}: No material assigned. " +
                             $"Assign a Material (Built-in: Unlit/Transparent) to render the disc.");

        // Built-in 2D: avoid lighting/shadow weirdness
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;

        // Start hidden until first event arrives
        _meshRenderer.enabled = false;

        // NOTE: sortingLayerName/sortingOrder on MeshRenderer is not consistently respected in built-in 2D
        // so we rely on Z offset for visibility.
    }

    private void LateUpdate()
    {
        // Follow target in world space (optional)
        var t = followTarget != null ? followTarget : transform;

        if (followTarget != null)
        {
            if (useXZPlane)
            {
                // 3D top-down
                transform.position = new Vector3(t.position.x, t.position.y + planeOffset, t.position.z);
            }
            else
            {
                // 2D: XY plane, push Z so it renders "above" background sprites (depending on camera)
                transform.position = new Vector3(t.position.x, t.position.y, zOffset2D + planeOffset);
            }
        }

        // Only apply scale if currently visible (optional micro-opt)
        if (_meshRenderer.enabled)
            ApplyRadius(_currentRadius);
    }

    private void OnEnable()
    {
        if (onNoiseRadiusChanged != null)
            onNoiseRadiusChanged.OnEventRaised += OnNoiseRadiusChanged;
    }

    private void OnDisable()
    {
        if (onNoiseRadiusChanged != null)
            onNoiseRadiusChanged.OnEventRaised -= OnNoiseRadiusChanged;
    }

    private void OnDestroy()
    {
        if (onNoiseRadiusChanged != null)
            onNoiseRadiusChanged.OnEventRaised -= OnNoiseRadiusChanged;
    }

    private void OnNoiseRadiusChanged(int radius)
    {
        _currentRadius = Mathf.Max(0f, radius * radiusMultiplier);

        if (_currentRadius <= hideAtOrBelow)
        {
            _meshRenderer.enabled = false;
            return;
        }

        _meshRenderer.enabled = true;
        ApplyRadius(_currentRadius);
    }

    private void ApplyRadius(float radius)
    {
        // Unit disc mesh has radius=1. We scale uniformly in the disc plane.
        if (useXZPlane)
            transform.localScale = new Vector3(radius, 1f, radius);
        else
            transform.localScale = new Vector3(radius, radius, 1f);
    }

    private static Mesh BuildUnitDiscMesh(int segments, bool xzPlane)
    {
        segments = Mathf.Clamp(segments, 8, 256);

        var mesh = new Mesh();
        mesh.name = "UnitDiscMesh";

        // Vertices: center + ring
        int vertCount = segments + 1;
        var vertices = new Vector3[vertCount];
        var uv = new Vector2[vertCount];
        var triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        float step = (Mathf.PI * 2f) / segments;

        for (int i = 0; i < segments; i++)
        {
            float a = i * step;
            float x = Mathf.Cos(a);
            float y = Mathf.Sin(a);

            // Unit circle radius = 1
            if (xzPlane)
                vertices[i + 1] = new Vector3(x, 0f, y);
            else
                vertices[i + 1] = new Vector3(x, y, 0f);

            uv[i + 1] = new Vector2(x * 0.5f + 0.5f, y * 0.5f + 0.5f);
        }

        // Triangles: fan from center
        // IMPORTANT: reverse winding to avoid backface culling invisibility in Built-in shaders
        for (int i = 0; i < segments; i++)
        {
            int triIndex = i * 3;
            int next = (i + 1) % segments;

            triangles[triIndex + 0] = 0;
            triangles[triIndex + 1] = next + 1; // swapped
            triangles[triIndex + 2] = i + 1;    // swapped
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
