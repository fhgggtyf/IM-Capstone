using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArc1 : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationsToLoad;
    [SerializeField] private SaveSystem _saveSystem = default;
    [SerializeField] private bool _showLoadScreen = default;

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO prologueFinished = default;

    private void Start()
    {
        prologueFinished.OnEventRaised += StartArc1Scene;

    }

    private void OnDestroy()
    {
        prologueFinished.OnEventRaised -= StartArc1Scene;
    }

    void StartArc1Scene()
    {
        _loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
    }

}
