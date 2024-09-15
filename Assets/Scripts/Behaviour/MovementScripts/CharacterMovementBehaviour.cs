using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementBehaviour : DynamicMovementBehaviour
{
    [SerializeField] private float _movementForce = 10.0f;
    [SerializeField] private float _jumpForce = 20.0f;
    [SerializeField] private float _jumpQueueTime = 0.2f;
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private GameObject _landEffectTemplate;

    private Vector2 _desiredDirection = Vector2.zero;
    private Rigidbody _rigidbody = null;
    private float _jumpQueue = 0.0f;
    private float _coyoteQueue = 0.0f;

    public Vector2 DesiredDirection
    {
        get { return _desiredDirection; }
        set { _desiredDirection = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate()
    {
        bool landed = false;
        bool wasGrounded = _isGrounded;
        base.FixedUpdate();

        landed = _isGrounded != wasGrounded && _isGrounded;
        if (landed && _landEffectTemplate != null)
            Instantiate(_landEffectTemplate, transform.position, transform.rotation);

        HandleMovement();

        JumpBuffer();
        CoyoteBuffer();
    }

    // Input buffer the jump right before landing
    private void JumpBuffer()
    {
        _jumpQueue -= Time.deltaTime;

        if (_jumpQueue > 0 && IsGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _jumpQueue = 0;
        }
    }

    // Players just getting of an edge has a slight chance to still jump
    // Helps with platformer
    private void CoyoteBuffer()
    {
        if (_isGrounded)
            _coyoteQueue = _coyoteTime;
        else
            _coyoteQueue -= Time.deltaTime;
    }

    protected virtual void HandleMovement()
    {
        if (_rigidbody == null) return;

        Vector3 movement = _desiredDirection.normalized * _movementForce;

        if (movement.magnitude > 5)
            _rigidbody.AddForce(movement);
    }
    public void Jump()
    {
        if (CanJump())
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        else
            _jumpQueue = _jumpQueueTime;
    }

    private bool CanJump()
    {
        if (_isGrounded)
            return true;

        if (_coyoteQueue > 0)
            return true;
        
        return false;
    }
}
