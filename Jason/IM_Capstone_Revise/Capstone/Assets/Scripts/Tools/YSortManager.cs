using UnityEngine;

[ExecuteAlways]
public class YSortManager : MonoBehaviour
{
    [SerializeField] private int sortingOrderBase = 0;
    [SerializeField] private int precision = 100;

    private YSortTarget[] targets;

    private void Reset()
    {
        targets = GetComponentsInChildren<YSortTarget>();
    }

    private void LateUpdate()
    {
        if (targets == null) return;

        foreach (var target in targets)
        {
            if (target == null) continue;

            int order = sortingOrderBase - Mathf.RoundToInt(target.SortPoint.position.y * precision);

            var renderers = target.Renderers;
            if (renderers == null) continue;

            foreach (var sr in renderers)
            {
                if (sr == null) continue;
                sr.sortingOrder = order;
            }
        }
    }
}