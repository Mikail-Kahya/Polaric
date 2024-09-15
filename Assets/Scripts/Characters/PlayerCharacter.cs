using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private InputActionReference _movementAction;
    [SerializeField] private InputActionReference _flyAction;

    [SerializeField] private UnityEvent _onSwitch;
    [SerializeField] private UnityEvent _onLand;

    private InputAction _jumpAction;
    private InputAction _switchAction;

    private Magnet _magnet;
    private MagneticSwitchingBehaviour _switchBehaviour;

    private bool _wasGrounded = false;

    protected override void Awake()
    {
        base.Awake();

        _magnet = GetComponent<Magnet>();
        _switchBehaviour = GetComponent<MagneticSwitchingBehaviour>();

        // check if object exists before using it
        if (_inputAsset == null) return;

        // find actionmaps in the asset and assign them
        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _switchAction = _inputAsset.FindActionMap("Gameplay").FindAction("Switch");

        // bind a callback to it instead of continously monitoring input
        _jumpAction.performed += HandleJumpInput;
        _switchAction.performed += HandleSwichingInput;
    }

    private void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
        _switchAction.performed -= HandleSwichingInput;
    }

    private void OnEnable()
    {
        // enable asset, is off by default
        if (_inputAsset == null) return;

        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        // cleanup
        if (_inputAsset == null) return;

        _inputAsset.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckLanding();
        HandleInput();
    }

    private void HandleInput()
    {
        if(_movementBehaviour == null)
            return;

        _movementBehaviour.DesiredDirection = Vector3.zero;

        HandleMovementInput();
        HandleFlyInput();

    }
    private void HandleMovementInput()
    {
        if (_movementAction == null)
            return;

        // movement action
        float movementInput = _movementAction.action.ReadValue<float>();
        _movementBehaviour.DesiredDirection += movementInput * Vector2.right;
    }

    private void HandleFlyInput()
    {
        if(_flyAction == null || !_magnet.IsAttractedToStatic)
            return;

        // fly action
        float flyInput = _flyAction.action.ReadValue<float>();
        _movementBehaviour.DesiredDirection += flyInput * Vector2.up;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        _movementBehaviour.Jump();
    }

    private void HandleSwichingInput(InputAction.CallbackContext context)
    {
        if (_switchBehaviour.Switch() && _magnet.IsActive)
            _onSwitch?.Invoke();

    }

    private void CheckLanding()
    {
        // Play the landing audio just once on initial landing (gets used in update)
        if (_movementBehaviour.IsGrounded)
        {
            if (_movementBehaviour.IsGrounded != _wasGrounded)
                _onLand?.Invoke();
        }

        _wasGrounded = _movementBehaviour.IsGrounded;
    }
}
