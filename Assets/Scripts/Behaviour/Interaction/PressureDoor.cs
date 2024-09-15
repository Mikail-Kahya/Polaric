using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Future steps in potential levels would be an addition of extra buttons => list
// Door does not remain open and closes again
public class PressureDoor : MonoBehaviour
{
    private DoorMovementBehaviour _doorMovementBehaviour;
    private PressureButton _button;
    
    private Material _material;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    private AudioSource _audioSource;

    private bool _lastActiveState = false;
    
    private void Awake()
    {
        _doorMovementBehaviour = GetComponentInChildren<DoorMovementBehaviour>();
        _button = GetComponentInChildren<PressureButton>();
        _audioSource = GetComponent<AudioSource>();

        _material = GetComponentInChildren<MeshRenderer>().material;

        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            renderer.sharedMaterial = _material;
    }

    private void Update()
    {
        // When an object with a rigidbody stands on the button, then open the door
        _material.color = (_button.IsPressed) ? _activeColor : _inactiveColor;

        if (_lastActiveState != _button.IsPressed)
        {
            if (_button.IsPressed)
                _audioSource?.Play();

            _doorMovementBehaviour.Toggle();
        }

        _lastActiveState = _button.IsPressed;
    }
}
