using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DynamicMovementBehaviour : MovementBehaviour
{
    [SerializeField] private PhysicMaterial _airPhysics;
    [SerializeField] private PhysicMaterial _groundedPhysics;
    private Collider _collider;

    protected bool _isGrounded = true;
    private const float GROUND_CHECK_DISTANCE = 0.1f;
    private const string GROUND_LAYER = "Ground";

    public bool IsGrounded
    {
        get { return _isGrounded; }
    }

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    protected virtual void FixedUpdate()
    {
        CheckGrounded();
        _collider.material = (_isGrounded) ? _groundedPhysics : _airPhysics;
    }

    protected void CheckGrounded()
    {
        if (_collider == null)
            return;

        bool tempGrounded = Physics.Raycast(transform.position + Vector3.up * GROUND_CHECK_DISTANCE * 0.5f, Vector3.down,
            GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER)); // cast on ground layer

        if (tempGrounded == _isGrounded)
            return;

        _isGrounded = tempGrounded;
    }
}
