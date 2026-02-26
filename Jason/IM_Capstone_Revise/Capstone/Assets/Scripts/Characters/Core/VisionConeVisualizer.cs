using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VisionCone visionCone;

    [Header("Mesh")]
    [Range(3, 256)] public int segments = 60;
    public Material material;

    [Header("Occlusion")]
    [Tooltip("Layers that can block the vision cone mesh (include your hide-behind objects layer(s)).")]
    [SerializeField] private LayerMask occluderMask = ~0;

    [Header("2D draw order")]
    public float zOffset2D = -1.5f;

    private MeshFilter _mf;
    private MeshRenderer _mr;
    private Mesh _mesh;

    private void Awake()
    {
        _mf = GetComponent<MeshFilter>();
        _mr = GetComponent<MeshRenderer>();

        _mesh = new Mesh { name = "VisionConeMesh" };
        _mf.sharedMesh = _mesh;

        if (material != null) _mr.sharedMaterial = material;

        _mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _mr.receiveShadows = false;

        if (visionCone == null)
            visionCone = GetComponentInParent<VisionCone>();
    }

    private void LateUpdate()
    {
        if (visionCone == null) return;

        // keep it on top in 2D
        var p = transform.position;
        transform.position = new Vector3(p.x, p.y, zOffset2D);

        BuildConeMesh(
            visionCone.GetOriginWorld(),
            visionCone.GetForwardWorld(),
            visionCone.GetRange(),
            visionCone.GetAngleDeg(),
            visionCone.playerTransform
        );
    }

    private void BuildConeMesh(
        Vector2 origin,
        Vector2 forward,
        float range,
        float angleDeg,
        Transform playerTransform
    )
    {
        if (range <= 0f || angleDeg <= 0f)
        {
            _mesh.Clear();
            return;
        }

        // vertices: origin + arc points
        int vCount = segments + 2;
        Vector3[] verts = new Vector3[vCount];
        int[] tris = new int[segments * 3];

        verts[0] = Vector3.zero; // local origin

        float half = angleDeg * 0.5f;
        float step = angleDeg / segments;

        // build in local space aligned with +X, then rotate to forward
        Quaternion rotToForward = Quaternion.FromToRotation(Vector2.right, forward.normalized);

        // For each segment, raycast and clamp range if a CanHideBehind object is hit.
        for (int i = 0; i <= segments; i++)
        {
            float a = -half + step * i;

            // local dir around +X
            Vector2 dir = Quaternion.Euler(0, 0, a) * Vector2.right;
            // rotate to NPC forward
            dir = rotToForward * dir;

            float currentRange = range;

            // raycast in WORLD
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, range, occluderMask);

            if (hits != null && hits.Length > 0)
            {
                // RaycastAll is returned in ascending distance in Unity (usually),
                // but we still guard by picking the closest valid occluder.
                float closest = range;

                foreach (var hit in hits)
                {
                    if (hit.collider == null) continue;
                    if (hit.collider.isTrigger) continue;

                    // ignore player collider(s)
                    if (playerTransform != null &&
                        (hit.collider.transform == playerTransform ||
                         hit.collider.transform.IsChildOf(playerTransform)))
                    {
                        continue;
                    }

                    // ONLY occlude on HideInteractable with CanHideBehind
                    HideInteractable hide = hit.collider.GetComponent<HideInteractable>();
                    if (hide != null && hide.CanHideBehind)
                    {
                        if (hit.distance < closest)
                            closest = hit.distance;
                    }
                }

                currentRange = closest;
            }

            // vertex in LOCAL space (mesh lives at origin; we move the object to origin)
            Vector2 pt = dir.normalized * currentRange;
            verts[i + 1] = new Vector3(pt.x, pt.y, 0f);
        }

        // triangle fan
        for (int i = 0; i < segments; i++)
        {
            int t = i * 3;
            tris[t + 0] = 0;
            tris[t + 1] = i + 1;
            tris[t + 2] = i + 2;
        }

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();

        // move mesh object to world origin point
        transform.position = new Vector3(origin.x, origin.y, zOffset2D);
    }
}
