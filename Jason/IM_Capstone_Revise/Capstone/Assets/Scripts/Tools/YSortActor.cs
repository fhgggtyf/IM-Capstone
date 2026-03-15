using UnityEngine;


public class YSortActor : MonoBehaviour
{
    [SerializeField] private int sortingOrderBase = 0;
    [SerializeField] private float offset = 0f;
    [SerializeField] private int precision = 100;
    [SerializeField] private Transform sortPoint;

    private SpriteRenderer[] _renderers;
    private int[] _initialOffsets;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _initialOffsets = new int[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _initialOffsets[i] = _renderers[i].sortingOrder;
        }
    }

    private void LateUpdate()
    {
        float y = sortPoint != null ? sortPoint.position.y : transform.position.y;
        int rootOrder = sortingOrderBase - Mathf.RoundToInt((y + offset) * precision);

        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] == null) continue;
            _renderers[i].sortingOrder = rootOrder + _initialOffsets[i];
        }
    }
}