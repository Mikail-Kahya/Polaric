using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private float _volume = 1.0f;
    private AudioSource _source;
    private Vector3 _spawn = Vector3.zero;

    public Vector3 Spawn
    {
        get { return _spawn; }
        set { _spawn = value; }
    }

    private void Awake()
    {
        _spawn += transform.position;
        
        // Add component because objects could have multiple sources (e.g player)
        _source = gameObject.AddComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.volume = _volume;
        _source.clip = _deathSound;
    }

    private void OnDestroy()
    {
        Destroy( _source );
    }

    public void Respawn()
    {
        transform.position = _spawn;

        // Remove velocity to fully reset the objects 
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.velocity = Vector3.zero;

        _source.Play();
    }
}
