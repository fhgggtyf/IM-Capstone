using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawnManager : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private List<NonPlayerCharacter> _npcPrefab = default;
    [SerializeField] private GameObject _npcSpawnRoot = default;
    [SerializeField] private PatrolRoute _patrolRoute = default;
    //[SerializeField] private List<TransformAnchor> _npcTransformAnchors = default;
    //[SerializeField] private List<TransformEventChannelSO> _npcInstantiatedChannels = default;

    [Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default; //Raised by SceneLoader when the scene is set to active

    [SerializeField] private VoidEventChannelSO _patrolRootSetChannel = default;

    private List<Transform> _enemySpawnLocations;
	//private List<Transform> _neutralSpawnLocations;

	private void Awake()
	{
        _enemySpawnLocations = new List<Transform>();

		foreach (Transform child in _npcSpawnRoot.transform)
		{
			_enemySpawnLocations.Add(child);
		}
	}

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += SpawnEnemies;
	}

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= SpawnEnemies;

        //foreach (TransformAnchor npcTransformAnchor in _npcTransformAnchors)
        //{
        //    npcTransformAnchor.Unset();
        //}

    }

    private void SpawnEnemies()
    {
        Debug.Log("NavMesh verts: " + NavMesh.CalculateTriangulation().vertices.Length);


        foreach (Transform spawn in _enemySpawnLocations)
        {
            DebugNavmeshDistance(spawn.position);

            // Find a nearby point on the NavMesh (increase radius if needed)
            if (!NavMesh.SamplePosition(spawn.position, out NavMeshHit hit, 50f, NavMesh.AllAreas))
            {
                Debug.LogWarning($"[SpawnEnemies] No NavMesh found near spawn '{spawn.name}' at {spawn.position}");
                continue;
            }

            foreach (NonPlayerCharacter npcPrefab in _npcPrefab)
            {
                // Spawn exactly on the NavMesh
                NonPlayerCharacter enemyInstance = Instantiate(npcPrefab, hit.position, spawn.rotation);

                // Extra safety: if it has a NavMeshAgent, warp it onto the mesh position
                var agent = enemyInstance.GetComponent<NavMeshAgent>();
                if (agent != null && agent.enabled)
                {
                    agent.Warp(hit.position);
                }

                enemyInstance.patrolRoute = _patrolRoute;
            }

            // If this event is ¡°the patrol root is ready for this spawn point¡±, raise once per location
            _patrolRootSetChannel.RaiseEvent();
        }
    } 
    static void DebugNavmeshDistance(Vector3 pos)
    {
        var tri = NavMesh.CalculateTriangulation();
        float best = float.PositiveInfinity;
        Vector3 bestV = default;

        foreach (var v in tri.vertices)
        {
            float d = Vector3.Distance(pos, v);
            if (d < best) { best = d; bestV = v; }
        }

        //Debug.Log($"Closest NavMesh vertex distance = {best:F3}, closest vertex = {bestV}, pos = {pos}");
    }
}



