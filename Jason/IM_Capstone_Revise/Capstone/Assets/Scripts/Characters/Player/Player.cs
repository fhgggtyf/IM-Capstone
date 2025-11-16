using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] public Weapon[] weapons;

    [SerializeField] public LayerMask whatIsEnemy;

    //These fields are read and manipulated by the StateMachine actions
    [NonSerialized] public Vector2 InputVector;
    [NonSerialized] public bool jumpInput;
    [NonSerialized] public bool[] attackInput;
    [NonSerialized] public bool isRunningPrep;
    [NonSerialized] public bool isRunning;
    [NonSerialized] public bool isCrouching;
    [NonSerialized] public bool isRolling;
    [NonSerialized] public bool isAbilityFinished;
    [NonSerialized] public int jumpCount;
    [SerializeField] public bool jumpIncremented;
    [NonSerialized] public bool isClimbing;

    private void Awake()
    {
        foreach (Weapon i in weapons)
        {
            i.SetCore(Core);
        }

        attackInput = new bool[2];
    }

    private void OnEnable()
    {
        animationEventHandler.OnFinish += OnAbilityFinished;
        _inputReader.MoveEvent += OnMove;
        _inputReader.MoveCanceledEvent += OnMoveCanceled;
        _inputReader.JumpEvent += OnJumpInitiated;
        _inputReader.JumpCanceledEvent += OnJumpCanceled;
        _inputReader.RunPrepEvent += OnRunPrep;
        _inputReader.InteractEvent += OnInteract;
        _inputReader.CrouchEvent += OnCrouch;
        _inputReader.CrouchCanceledEvent += OnCrouchCanceled;
        _inputReader.RollEvent += OnRoll;
        _inputReader.RollCanceledEvent += OnRollCanceled;
        _inputReader.PrimaryAttackEvent += OnPrimaryAttack;
        _inputReader.PrimaryAttackCanceledEvent += OnPrimaryAttackCanceled;
        _inputReader.SecondaryAttackEvent += OnSecondaryAttack;
        _inputReader.SecondaryAttackCanceledEvent += OnSecondaryAttackCanceled;
        _inputReader.GameplayInputToggled += _inputReader.BlockGameplayInput;
    }

    private void OnDisable()
    {
        animationEventHandler.OnFinish -= OnAbilityFinished;
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJumpInitiated;
        _inputReader.JumpCanceledEvent -= OnJumpCanceled;
        _inputReader.RunPrepEvent -= OnRunPrep;
        _inputReader.InteractEvent -= OnInteract;
        _inputReader.CrouchEvent -= OnCrouch;
        _inputReader.CrouchCanceledEvent -= OnCrouchCanceled;
        _inputReader.RollEvent -= OnRoll;
        _inputReader.RollCanceledEvent -= OnRollCanceled;
        _inputReader.PrimaryAttackEvent -= OnPrimaryAttack;
        _inputReader.PrimaryAttackCanceledEvent -= OnPrimaryAttackCanceled;
        _inputReader.SecondaryAttackEvent -= OnSecondaryAttack;
        _inputReader.SecondaryAttackCanceledEvent -= OnSecondaryAttackCanceled;
        _inputReader.GameplayInputToggled -= _inputReader.BlockGameplayInput;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAbilityFinished() => isAbilityFinished = true;

    private void OnMove(Vector2 inputMovement)
    {
        InputVector = new Vector2(Math.Sign(inputMovement.x) * (Math.Abs(inputMovement.x) >= 1 ? Math.Abs(inputMovement.x) : 1), Math.Sign(inputMovement.y) * (Math.Abs(inputMovement.y) >= 1 ? Math.Abs(inputMovement.y) : 1));
        if (isRunningPrep)
        {
            isRunning = true;
        }
    }
    private void OnMoveCanceled()
    {
        Debug.Log("Move Canceled");
        InputVector = new Vector2(0, 0);
        if (isRunningPrep)
        {
            isRunningPrep = false;
            isRunning = false;
        }
    }
    private void OnRunPrep(Vector2 inputMovement)
    {
        isRunningPrep = true;
        InputVector = new Vector2(Math.Sign(inputMovement.x) * (Math.Abs(inputMovement.x) >= 1 ? Math.Abs(inputMovement.x) : 1), Math.Sign(inputMovement.y) * (Math.Abs(inputMovement.y) >= 1 ? Math.Abs(inputMovement.y) : 1));
        StartCoroutine(DoubleTapDelay());
    }

    IEnumerator DoubleTapDelay()
    {
        // �ȴ�0.5��  
        yield return new WaitForSeconds(0.5f);

        if (InputVector.x == 0 && Core.GetCoreComponent<CollisionSenses>().Ground)
        {
            isRunningPrep = false;
        }
    }

    private void OnCrouch()
    {
        isCrouching = true;
    }
    private void OnCrouchCanceled()
    {
        isCrouching = false;
    }

    private void OnJumpInitiated()
    {
        jumpInput = true;
    }

    private void OnJumpCanceled()
    {
        jumpInput = false;
    }

    private void OnRoll()
    {
        isRolling = true;
    }

    private void OnRollCanceled()
    {
        isRolling = false;
    }

    private void OnInteract()
    {
        Core.GetCoreComponent<InteractableDetector>().TryInteract();
    }

    private void OnPrimaryAttack()
    {
        attackInput[(int)CombatInputs.primary] = true;
    }

    private void OnSecondaryAttack()
    {
        attackInput[(int)CombatInputs.secondary] = true;
    }

    private void OnPrimaryAttackCanceled()
    {
        attackInput[(int)CombatInputs.primary] = false;
    }

    private void OnSecondaryAttackCanceled()
    {
        attackInput[(int)CombatInputs.secondary] = false;
    }

    public void UseAttackInput(int i) => attackInput[i] = false;
}
