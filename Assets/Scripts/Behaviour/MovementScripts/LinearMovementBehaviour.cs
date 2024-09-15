using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovementBehaviour : MovementBehaviour
{
    // Different modes of choosing the next point to move to
    enum Mode
    {
        alternating, // 1 - 2 - 3 - 4 - 3 - 2 ....
        looping, // 1 - 2 - 3 - 4 - 1 - 2 - ....
        teleportLoop // 1 - 2 - 3 - 4 |||| 1 - 2 - ....
    }

    [SerializeField] protected List<Vector2> _pathPoints = new List<Vector2>();
    private int _pathIndex = 0;

    [SerializeField] private Mode _mode = Mode.alternating;

    protected Vector2 _previousPosition;
    protected Vector2 _nextPosition;
    protected float _timer;
    private bool _isReversing = false;

    protected virtual void Awake()
    {
        UpdatePath();
        gameObject.isStatic = false;
    }

    const float DISTANCE_EPS = 0.25f;

    protected virtual void FixedUpdate()
    {
        _timer += Time.deltaTime;
        // update position
        gameObject.transform.position = Vector2.Lerp(_previousPosition, _nextPosition, _timer * _speed);
    }

    protected virtual void Update()
    {
        // if player is close to the end position, update the path
       float distance = (_nextPosition - (Vector2)gameObject.transform.position).sqrMagnitude;
       if (distance < DISTANCE_EPS)
           UpdatePath();
    }

    private void UpdatePath()
    {
        // change start and end point of lerp
        _previousPosition = _pathPoints[_pathIndex];

        switch (_mode)
        {
            case Mode.alternating:
                AlternatingPath();
                break; 
            case Mode.looping:
                LoopingPath();
                break;
            case Mode.teleportLoop:
                TeleportLoopingPath();
                break;
        }

        _timer = 0;
        _nextPosition = _pathPoints[_pathIndex];
    }

    private void AlternatingPath ()
    {
        // check again if it needs to reverse
        // 1 - 2 - 3 - 4 - 3 - 2 ....
        
        if (_isReversing) 
            _isReversing = !(_pathIndex == 0);
        else
            _isReversing = _pathIndex == _pathPoints.Count - 1;

        if (_isReversing)
            --_pathIndex;
        else
            ++_pathIndex;
    }

    private void LoopingPath ()
    {
        // 1 - 2 - 3 - 4 - 1 - 2 - ....
        ++_pathIndex;
        _pathIndex %= _pathPoints.Count;
    }

    private void TeleportLoopingPath ()
    {
        // 1 - 2 - 3 - 4 |||| 1 - 2 - ....
        ++_pathIndex;
        if (_pathIndex == _pathPoints.Count)
        {
            _previousPosition = _pathPoints[0];
            _pathIndex = 1;
        }
    }

    // add points in the editor instead
    [ContextMenu("Add point")]
    protected virtual void AddPoint()
    {
        _pathPoints.Add(gameObject.transform.position);
    }
}
