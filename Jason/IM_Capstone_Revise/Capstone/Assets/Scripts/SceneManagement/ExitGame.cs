using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] private SaveSystem _saveSystem = default;
    [SerializeField] private VoidEventChannelSO _exitGame = default;

    private void OnEnable()
    {
        _exitGame.OnEventRaised += Exit;
    }

    private void OnDisable()
    {
        _exitGame.OnEventRaised -= Exit;
    }

    private void Exit()
    {
        _saveSystem.SaveDataToDisk();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
