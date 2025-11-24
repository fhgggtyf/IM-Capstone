using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private InputReader _inputReader = default;

    [SerializeField] public LayerMask whatIsEnemy;

    //These fields are read and manipulated by the StateMachine actions
    [NonSerialized] public Vector2 InputVector;
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
    }

    private void OnEnable()
    {
        animationEventHandler.OnFinish += OnAbilityFinished;
        _inputReader.MoveEvent += OnMove;
        _inputReader.MoveCanceledEvent += OnMoveCanceled;
        _inputReader.InteractEvent += OnInteract;
        _inputReader.HoldBreathEvent += OnHoldBreath;
        _inputReader.HoldBreathCanceledEvent += OnHoldBreathCanceled;
        _inputReader.GameplayInputToggled += _inputReader.BlockGameplayInput;
    }

    private void OnDisable()
    {
        animationEventHandler.OnFinish -= OnAbilityFinished;
        _inputReader.MoveEvent -= OnMove;
        _inputReader.InteractEvent -= OnInteract;
        _inputReader.HoldBreathEvent -= OnHoldBreath;
        _inputReader.HoldBreathCanceledEvent -= OnHoldBreathCanceled;
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

    private void OnHoldBreath()
    {
        isCrouching = true;
    }
    private void OnHoldBreathCanceled()
    {
        isCrouching = false;
    }

    private void OnInteract()
    {
        Core.GetCoreComponent<InteractableDetector>().TryInteract();
    }
}
