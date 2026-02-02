using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BindAgentToNavMeshOnSpawn : MonoBehaviour
{
    [Header("How far we search for the nearest walkable point")]
    [SerializeField] private float sampleRadius = 2.0f;

    [Header("Retry because NavMesh may appear after additive loads / one-frame delays")]
    [SerializeField] private int maxFramesToRetry = 10;

    [SerializeField] private int areaMask = NavMesh.AllAreas;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        // Start binding when spawned/enabled.
        StartCoroutine(EnsureOnNavMesh());
    }

    private IEnumerator EnsureOnNavMesh()
    {
        // If we enable and set destination while not on NavMesh, Unity will complain.
        // So we try to place the agent first.
        for (int i = 0; i < maxFramesToRetry; i++)
        {
            if (_agent != null && _agent.enabled && _agent.isOnNavMesh)
                yield break; // already good 

            // Sample nearest point on the NavMesh
            if (NavMesh.SamplePosition(transform.position, out var hit, sampleRadius, areaMask))
            {
                // Warp places the agent onto the navmesh immediately
                // (and sets its internal nav position). 
                _agent.Warp(hit.position);
                yield break;
            }

            // Wait a frame and try again (useful for additive scene load timing)
            yield return null;
        }

        Debug.LogWarning(
            $"{name}: Failed to place NavMeshAgent on NavMesh (pos={transform.position}, radius={sampleRadius}). " +
            $"Falling back to non-NavMesh movement.");
    }
}
