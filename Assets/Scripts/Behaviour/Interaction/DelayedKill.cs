using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mostly used for particle effects
public class DelayedKill : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
    private void Awake()
    {
        Invoke("Kill", _delay);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
