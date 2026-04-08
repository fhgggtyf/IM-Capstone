using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRestartGameMenu : MonoBehaviour
{
    [SerializeField] private Button _RestartGameButton = default;
    [SerializeField] private Button _ExitButton = default;

    public UnityAction RestartGameMenuAction;
    public UnityAction ExitButtonAction;

    public void NewGameButton()
    {
        RestartGameMenuAction.Invoke();
    }

    public void ContinueButton()
    {
        ExitButtonAction.Invoke();
    }
}
