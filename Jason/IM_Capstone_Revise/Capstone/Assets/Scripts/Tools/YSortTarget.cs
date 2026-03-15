using UnityEngine;

public class YSortTarget : MonoBehaviour
{
    public SpriteRenderer[] Renderers => _renderers;
    public Transform SortPoint => sortPoint != null ? sortPoint : transform;

    [SerializeField] private Transform sortPoint;
    [SerializeField] private SpriteRenderer[] _renderers;

    private void Reset()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>(true);
    }
}