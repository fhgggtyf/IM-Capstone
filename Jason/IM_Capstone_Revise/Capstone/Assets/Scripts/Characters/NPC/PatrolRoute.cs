using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public Transform[] Points { get; private set; }

    private void Awake()
    {
        // Auto-collect children as points (order = hierarchy order)
        Points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            Points[i] = transform.GetChild(i);
    }
}
