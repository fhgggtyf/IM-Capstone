using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private List<NonPlayerCharacter> _npcPrefab = default;
    [SerializeField] private GameObject _npcSpawnRoot = default;
	//[SerializeField] private List<TransformAnchor> _npcTransformAnchors = default;
	//[SerializeField] private List<TransformEventChannelSO> _npcInstantiatedChannels = default;

	[Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default; //Raised by SceneLoader when the scene is set to active

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
        foreach (Transform i in _enemySpawnLocations)
        {
            if (_npcPrefab.Count > 0)
            {
                NonPlayerCharacter enemyInstance = Instantiate(_npcPrefab[Random.Range(0, _npcPrefab.Count)], i.position, i.rotation);
            }
            else
            {
                Debug.LogWarning("No NPC in list");
                return;
            }

        }

    }
}
