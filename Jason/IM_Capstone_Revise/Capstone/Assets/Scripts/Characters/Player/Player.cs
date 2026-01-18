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
    [NonSerialized] public bool isRunning;
    [NonSerialized] public bool isHoldingBreath;
    [NonSerialized] public bool isCrouching;

    // Indicates whether the player is currently hiding inside a hideable object. When true
    // the state machine can transition into a hiding state which will pull the player
    // into the hideable's position and mute their noise.
    [NonSerialized] public bool isHiding;

    // The transform of the hideable object the player is currently hiding in. This will
    // be set by an interaction component (e.g. HideInteraction) when the player
    // interacts with a hideable object. Hide-related state actions use this to
    // reposition the player inside the hideable.
    [NonSerialized] public Transform hideTarget;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
        _inputReader.MoveCanceledEvent += OnMoveCanceled;
        _inputReader.InteractEvent += OnInteract;
        _inputReader.HoldBreathEvent += OnHoldBreath;
        _inputReader.HoldBreathCanceledEvent += OnHoldBreathCanceled;
        _inputReader.GameplayInputToggled += _inputReader.BlockGameplayInput;
    }

    private void OnDisable()
    {
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

    private void OnMove(Vector2 inputMovement)
    {
        InputVector = new Vector2(Math.Sign(inputMovement.x) * (Math.Abs(inputMovement.x) >= 1 ? Math.Abs(inputMovement.x) : 1), Math.Sign(inputMovement.y) * (Math.Abs(inputMovement.y) >= 1 ? Math.Abs(inputMovement.y) : 1));
    }
    private void OnMoveCanceled()
    {
        Debug.Log("Move Canceled");
        InputVector = new Vector2(0, 0);
    }

    private void OnHoldBreath()
    {
        isHoldingBreath = true;
    }
    private void OnHoldBreathCanceled()
    {
        isHoldingBreath = false;
    }

    private void OnInteract()
    {
        Core.GetCoreComponent<InteractableDetector>().TryInteract();
    }
}
