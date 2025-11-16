using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IMenusActions, GameInput.IDialoguesActions
{
    //[Space]
    //[SerializeField] private GameStateSO _gameStateManager;

    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them

    // Gameplay
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };
    public event UnityAction PrimaryAttackEvent = delegate { };
    public event UnityAction PrimaryAttackCanceledEvent = delegate { };
    public event UnityAction SecondaryAttackEvent = delegate { };
    public event UnityAction SecondaryAttackCanceledEvent = delegate { };
    public event UnityAction InteractEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
    public event UnityAction RollEvent = delegate { };
    public event UnityAction RollCanceledEvent = delegate { };
    public event UnityAction InventoryActionButtonEvent = delegate { };
    public event UnityAction SaveActionButtonEvent = delegate { };
    public event UnityAction ResetActionButtonEvent = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction MoveCanceledEvent = delegate { };
    public event UnityAction<Vector2> RunPrepEvent = delegate { };
    public event UnityAction CrouchEvent = delegate { };
    public event UnityAction CrouchCanceledEvent = delegate { };
    public event UnityAction<bool> GameplayInputToggled = delegate { };

    public event UnityAction<Vector2, bool> CameraMoveEvent = delegate { };
    public event UnityAction EnableMouseControlCameraEvent = delegate { };
    public event UnityAction DisableMouseControlCameraEvent = delegate { };

    public event UnityAction StartedRunning = delegate { };
    public event UnityAction StoppedRunning = delegate { };

    // Shared between menus and dialogues
    public event UnityAction MoveSelectionEvent = delegate { };

    // Dialogues
    public event UnityAction AdvanceDialogueEvent = delegate { };

    // Menus
    public event UnityAction MenuMouseMoveEvent = delegate { };
    public event UnityAction MenuClickButtonEvent = delegate { };
    public event UnityAction MenuUnpauseEvent = delegate { };
    public event UnityAction MenuPauseEvent = delegate { };
    public event UnityAction MenuCloseEvent = delegate { };
    public event UnityAction OpenInventoryEvent = delegate { }; 
    public event UnityAction CloseInventoryEvent = delegate { }; 
    public event UnityAction<float> TabSwitched = delegate { };

    // Cheats (has effect only in the Editor)
    public event UnityAction CheatMenuEvent = delegate { };

    private GameInput _gameInput;

    public bool GameplayInputBlocked { get; private set; }

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.Menus.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void BlockGameplayInput(bool block)
    {
        GameplayInputBlocked = block;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
        if (context.phase == InputActionPhase.Canceled)
            MoveCanceledEvent.Invoke();
    }

    public void OnRunPrep(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        if (context.phase == InputActionPhase.Performed)
            RunPrepEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        if (context.phase == InputActionPhase.Performed)
            JumpEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            JumpCanceledEvent.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent.Invoke();
        }

    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                PrimaryAttackEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                PrimaryAttackCanceledEvent.Invoke();
                break;
        }
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                SecondaryAttackEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                SecondaryAttackCanceledEvent.Invoke();
                break;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        if (context.phase == InputActionPhase.Performed)
            CrouchEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            CrouchCanceledEvent.Invoke();
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (GameplayInputBlocked) return;

        if (context.phase == InputActionPhase.Performed)
            RollEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            RollCanceledEvent.Invoke();

    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OpenInventoryEvent.Invoke();
        }
    }

    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CloseInventoryEvent.Invoke();
        }
    }

    public void DisableAllInput()
    {
        _gameInput.Menus.Disable();
        _gameInput.Gameplay.Disable();
        _gameInput.Dialogues.Disable();
    }

    public void EnableDialogueInput()
    {
        _gameInput.Menus.Enable();
        _gameInput.Gameplay.Disable();
        _gameInput.Dialogues.Enable();
    }

    public void EnableGameplayInput()
    {
        _gameInput.Menus.Disable();
        _gameInput.Dialogues.Disable();
        _gameInput.Gameplay.Enable();
        GameplayInputToggled.Invoke(false);
    }

    public void EnableMenuInput()
    {
        _gameInput.Dialogues.Disable();
        GameplayInputToggled.Invoke(true);
        _gameInput.Menus.Enable();
    }

    public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuCloseEvent.Invoke();
    }

    public void OnInventoryActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            InventoryActionButtonEvent.Invoke();
    }

    public void OnSaveActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SaveActionButtonEvent.Invoke();
    }

    public void OnResetActionButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ResetActionButtonEvent.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuPauseEvent.Invoke();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuClickButtonEvent.Invoke();
    }

    public void OnChangeTab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            TabSwitched.Invoke(context.ReadValue<float>());
    }

    public void OnMoveSelection(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MoveSelectionEvent.Invoke();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuMouseMoveEvent.Invoke();
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MenuUnpauseEvent.Invoke();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {

    }

    public void OnPoint(InputAction.CallbackContext context)
    {

    }

    public void OnRightClick(InputAction.CallbackContext context)
    {

    }

    public void OnNavigate(InputAction.CallbackContext context)
    {

    }

    public void OnAdvanceDialogue(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
            AdvanceDialogueEvent.Invoke();
    }


}

public enum CombatInputs
{
    primary,
    secondary
}

