using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follow-up on audio spawn
// Play the audio in the world and destroy it
// Used when an object destroys itself and audio still needs to play E.g. pickup
public class AudioPlay : MonoBehaviour
{
    private bool _played;

    private void Update()
    {
        if (_played)
            return;

        AudioSource source = GetComponent<AudioSource>();
        source.Play();
        Invoke("Kill", source.clip.length);
        _played = true;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
