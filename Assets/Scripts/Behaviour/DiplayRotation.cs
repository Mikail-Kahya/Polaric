using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplayRotation : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = transform.rotation;
        rotation.y = Mathf.Lerp(0 , 1, (Time.time * _rotationSpeed) % 1);
        transform.rotation = rotation;
    }
}
