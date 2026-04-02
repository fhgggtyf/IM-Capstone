using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(NavMeshObstacle))]
public class NavMeshObstacleSync : MonoBehaviour
{
    private Collider2D _collider;
    private NavMeshObstacle _obstacle;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _obstacle = GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        if (!_collider.isActiveAndEnabled)
        {
            return; // 如果Collider被禁用，跳过更新，避免导航异常
        }

        // 同步位置与旋转，确保移动中障碍物实时跟随
        _obstacle.transform.position = transform.position;
        _obstacle.transform.rotation = transform.rotation;

        // 根据Collider2D类型动态匹配Obstacle形状
        if (_collider is BoxCollider2D box)
        {
            _obstacle.shape = NavMeshObstacleShape.Box;

            _obstacle.center = new Vector3(box.offset.x, box.offset.y, 0); // 2D坐标转换为3D
            _obstacle.size = new Vector3(box.size.x, box.size.y, 1);
        }
        else
        {
            // 未知类型，使用默认包围盒
            _obstacle.shape = NavMeshObstacleShape.Box;
            _obstacle.size = new Vector3(_collider.bounds.size.x, _collider.bounds.size.y, 1);
        }

        // 关键设置：必须开启雕刻，否则不会影响NavMesh
        _obstacle.carving = true; // ✅ 必须开启！这是“加入导航”的核心开关
    }

    // 编辑器中修改Collider后自动刷新，提升开发效率
    private void OnValidate()
    {
        if (_obstacle != null && _collider != null)
        {
            Update();
        }
    }
}
