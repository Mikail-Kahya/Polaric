using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Extention of lerping

public class DoorMovementBehaviour : LinearMovementBehaviour
{
    private bool _isPressed = false;
    private bool _isActive = false;

    protected override void Awake()
    {
        base.Awake();
        transform.position = _previousPosition;
    }

    protected override void FixedUpdate()
    {
        // Only moves when active compared to continuous
        if (_isActive)
            base.FixedUpdate();
    }

    protected override void Update()
    {
    }

    public bool Toggle()
    {
        if (_pathPoints.Count < 2)
            return false;

        _isActive = true;
        _timer = 0;
        _isPressed = !_isPressed;

        _nextPosition = (_isPressed) ? _pathPoints[1] : _pathPoints[0];
        _previousPosition = transform.position;

        return true;
    }

    // use unity editor to add points
    [ContextMenu("Add point")]
    protected override void AddPoint()
    {
        // Limit the points being added
        if (_pathPoints.Count >= 2)
            return;
        _pathPoints.Add(gameObject.transform.position);
    }
}
