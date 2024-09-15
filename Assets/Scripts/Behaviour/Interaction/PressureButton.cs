using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any object that is a rigidbody will interact with the button
// Weight does not matter, just collision

public class PressureButton : MonoBehaviour
{
    private List<GameObject> _pressureObjList = new List<GameObject>();
    private bool _pressed = false;

    [SerializeField] float _pressIndentDistance = 0.1f;
    private Vector3 _originalPos = Vector3.zero;

    public bool IsPressed { get { return _pressed; } }

    private void Awake()
    {
        _originalPos += transform.position;
    }

    private void Update()
    {
        Vector3 pressedPos = Vector3.zero + _originalPos;
        pressedPos.y -= _pressIndentDistance;

        transform.position = (_pressed) ? pressedPos : _originalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != Magnet.Tag)
            return;

        _pressureObjList.Add(other.gameObject);
        _pressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != Magnet.Tag)
            return;

        _pressureObjList.Remove(other.gameObject);
        _pressed = _pressureObjList.Count != 0;
    }
}
