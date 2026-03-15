using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>
public class LocationExit : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationToLoad = default;
    [SerializeField] private PathSO _leadsToPath = default;
    [SerializeField] private PathStorageSO _pathStorage = default; //This is where the last path taken will be stored
    [SerializeField] private List<StepSO> _prerequisites = default; //The steps that need to be completed for the exit to work, if any

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _locationExitLoadChannel = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_prerequisites != null && _prerequisites.Count != 0)
        {
            foreach (StepSO obj in _prerequisites)
            {
                if (!obj.IsDone)
                {
                    return;
                }
            }
        }

        if (collision.CompareTag("Player"))
        {
            _pathStorage.lastPathTaken = _leadsToPath;
            _locationExitLoadChannel.RaiseEvent(_locationToLoad, false, true);
        }

    }
}
