using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraArm : MonoBehaviour
{
    private CinemachineComponentBase _cameraArmComponent = null;
    private Rigidbody _playerRigidbody = null;

    [SerializeField] float _minCameraDistance = 60.0f;
    [SerializeField] float _maxCameraDistance = 100.0f;
    [SerializeField] float _paningThreshold = 10.0f;
    [SerializeField] float _maxSpeed = 30.0f;

    public GameObject Player
    {
        set { _playerRigidbody = value.GetComponent<Rigidbody>(); }
    }

    private void Awake()
    {
        _cameraArmComponent = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent(CinemachineCore.Stage.Body);
    }

    private void Update()
    {
        // Give a zooming out effect when moving at faster speeds
        if (_playerRigidbody == null)
            return;

        float speed = Mathf.Abs(_playerRigidbody.velocity.x);
        float cameraDistance = _minCameraDistance;

        if (_paningThreshold < speed)
        {
            float panRatio = Mathf.Min(1.0f, speed / _maxSpeed);
            cameraDistance =  Mathf.Max(_minCameraDistance, panRatio * _maxCameraDistance);
        }
        
        if (_cameraArmComponent is CinemachineFramingTransposer)
            (_cameraArmComponent as CinemachineFramingTransposer).m_CameraDistance = cameraDistance;
    }
}
