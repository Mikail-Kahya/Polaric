using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpawn : MonoBehaviour
{
    [SerializeField] GameObject _audioObjTemplate;
    [SerializeField] AudioClip _audioClip;
    [SerializeField, Range(0.0f, 2.0f)] float _volume = 1.0f;
    [SerializeField, Range(0.0f, 3.0f)] float _pitch = 1.0f;

    public void Spawn()
    {
        GameObject audioObj = Instantiate(_audioObjTemplate);
        audioObj.transform.position = transform.position;
        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.clip = _audioClip;
        audioSource.volume = _volume;
        audioSource.pitch = _pitch;
    }
}
