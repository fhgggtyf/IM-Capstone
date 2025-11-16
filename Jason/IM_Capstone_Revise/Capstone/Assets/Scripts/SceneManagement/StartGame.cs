using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>
public class StartGame : MonoBehaviour
{
	[SerializeField] private GameSceneSO _locationsToLoad;
	[SerializeField] private SaveSystem _saveSystem = default;
	[SerializeField] private bool _showLoadScreen = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private LoadEventChannelSO _loadHome = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _onNewGameButton = default;
	[SerializeField] private VoidEventChannelSO _onContinueButton = default;

	private void Start()
	{
		_saveSystem.LoadSaveDataFromDisk();
		_onNewGameButton.OnEventRaised += StartNewGame;
		_onContinueButton.OnEventRaised += ContinuePreviousGame;
	}

	private void OnDestroy()
	{
		_onNewGameButton.OnEventRaised -= StartNewGame;
		_onContinueButton.OnEventRaised -= ContinuePreviousGame;
	}

	private void StartNewGame()
	{
		_saveSystem.WriteEmptySaveFile();;
		_saveSystem.SetNewGameData();
		_loadHome.RaiseEvent(_locationsToLoad, _showLoadScreen);
	}

	private void ContinuePreviousGame()
	{
		StartCoroutine(LoadSaveGame());
	}

	private void OnResetSaveDataPress()
	{
	}

	private IEnumerator LoadSaveGame()
	{
		yield return StartCoroutine(_saveSystem.LoadSavedInventory());

		//_saveSystem.LoadSavedQuestlineStatus();
		var locationGuid = _saveSystem.saveData._locationId;
		var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

		yield return asyncOperationHandle;

		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			LocationSO locationSO = asyncOperationHandle.Result;
			if(locationSO.sceneType == GameSceneSO.GameSceneType.Home)
            {
				_loadHome.RaiseEvent(locationSO, _showLoadScreen);
			}
            else
            {
				_loadLocation.RaiseEvent(locationSO, _showLoadScreen);
            }

		}
	}
}
