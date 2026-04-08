using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
	[SerializeField] private QuestManagerSO _questManager = default;
	[SerializeField] private GameStateSO _gameState = default;
	[SerializeField] private UIDialogueManager _dialogueManager = default;

    [SerializeField] private bool _showLoadScreen = default;

    [SerializeField] private UIPopup _popupPanel = default;

    [SerializeField] private VoidEventChannelSO _currentSceneUnloaded = default;
    [SerializeField] private VoidEventChannelSO _playerDead = default;
	[SerializeField] private SaveSystem _saveSystem = default;
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private VoidEventChannelSO _exitGameEvent = default;
	[SerializeField] private VoidEventChannelSO _unloadCurrentScene = default;

    [Header("Inventory")]
	[SerializeField] private InventorySO _inventory = default;

	private LocationSO _currentDeathLocation;
    private bool _pendingRestart = false;

    /// <summary>
    /// Event channels to broadcast on
    /// </summary>
    //[Header("Broadcasting on")]
    //[SerializeField] private VoidEventChannelSO _addRockCandyRecipeEvent = default;
    //[SerializeField] private VoidEventChannelSO _cerisesMemoryEvent = default;
    //[SerializeField] private VoidEventChannelSO _decideOnDishesEvent = default;

    private void Start()
	{
		StartGame();
	}

    private void OnEnable()
    {
        _playerDead.OnEventRaised += PlayerDied;
        _currentSceneUnloaded.OnEventRaised += OnCurrentSceneUnloaded;
    }

    private void OnDisable()
    {
        _playerDead.OnEventRaised -= PlayerDied;
        _currentSceneUnloaded.OnEventRaised -= OnCurrentSceneUnloaded;
    }

    void PlayerDied()
	{
		_currentDeathLocation = (LocationSO)SceneLoader.GetCurrentScene();

        ShowGameOverUI();

        //_loadLocation.RaiseEvent(_location, _showLoadScreen);
    }

	void ShowGameOverUI()
	{
        _popupPanel.ConfirmationResponseAction += RestartGamePopupResponse;
        _popupPanel.ClosePopupAction += HidePopup;

        _popupPanel.gameObject.SetActive(true);
        _popupPanel.SetPopup(PopupType.Restart);

    }

	void RestartGamePopupResponse(bool responseConfirmed)
	{
        _popupPanel.ConfirmationResponseAction -= RestartGamePopupResponse;
        _popupPanel.ClosePopupAction -= HidePopup;

        _popupPanel.gameObject.SetActive(false);

        if (responseConfirmed)
        {
            ConfirmRestart();
        }
        else
        {
            _exitGameEvent.RaiseEvent();
        }
    }

    void ConfirmRestart()
    {
        _pendingRestart = true;
        _unloadCurrentScene.RaiseEvent();
    }

    void OnCurrentSceneUnloaded()
    {
        if (!_pendingRestart || _currentDeathLocation == null)
            return;

        _pendingRestart = false;
        _loadLocation.RaiseEvent(_currentDeathLocation, _showLoadScreen);
    }

    void HidePopup()
    {
        _popupPanel.ClosePopupAction -= HidePopup;
        _popupPanel.gameObject.SetActive(false);

    }

    void StartGame()
	{
		_gameState.UpdateGameState(GameState.Gameplay);
        _dialogueManager.IsQuestCompleted = _questManager.IsStepCompleted;
        _questManager.StartGame();
	}
}
