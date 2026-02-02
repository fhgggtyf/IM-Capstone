using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VisionCone visionCone;

    [Header("Mesh")]
    [Range(3, 128)] public int segments = 30;
    public Material material;

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
            visionCone.GetAngleDeg()
        );
    }

    private void BuildConeMesh(Vector2 origin, Vector2 forward, float range, float angleDeg)
    {
        // vertices: origin + arc points
        int vCount = segments + 2;
        Vector3[] verts = new Vector3[vCount];
        int[] tris = new int[segments * 3];

        verts[0] = Vector3.zero; // local origin

        float half = angleDeg * 0.5f;
        float step = angleDeg / segments;

        // build in local space, aligned with +X, then rotate to forward
        Quaternion rotToForward = Quaternion.FromToRotation(Vector2.right, forward.normalized);

        for (int i = 0; i <= segments; i++)
        {
            float a = -half + step * i;
            Vector2 dir = Quaternion.Euler(0, 0, a) * Vector2.right;
            dir = rotToForward * dir;

            Vector2 pt = dir * range;
            verts[i + 1] = new Vector3(pt.x, pt.y, 0f);
        }

        // triangles fan
        for (int i = 0; i < segments; i++)
        {
            int t = i * 3;
            tris[t + 0] = 0;
            tris[t + 1] = i + 1;
            tris[t + 2] = i + 2;
        }

        // set mesh
        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();

        // position this mesh at origin
        transform.position = new Vector3(origin.x, origin.y, zOffset2D);
    }
}
