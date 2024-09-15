using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _playerTemplate = null;
    [SerializeField] private GameObject _followCameraArmTemplate = null;
    [SerializeField] private GameObject _cameraTemplate = null;
    [SerializeField] private GameObject _pauseUI = null;
    [SerializeField] private bool _isMagnetOn = true;

    private void Awake()
    {
        // Setup basic playing components: Player, camera, camerarm, Pausing
        GameObject player = Instantiate(_playerTemplate);
        player.GetComponent<Magnet>().IsActive = _isMagnetOn;
        GameObject followCameraArm = Instantiate(_followCameraArmTemplate);
        Instantiate(_cameraTemplate);

        CinemachineVirtualCamera virtualCamera = followCameraArm.GetComponent<CinemachineVirtualCamera>();
        CameraArm cameraArm = followCameraArm.GetComponent<CameraArm>();

        player.transform.position = gameObject.transform.position;

        if (virtualCamera != null)
            virtualCamera.Follow = player.transform;

        if (cameraArm != null && player != null)
            cameraArm.Player = player;

        Instantiate(_pauseUI);

        // Destroy because it never gets used again
        Destroy(gameObject);
    }
}
