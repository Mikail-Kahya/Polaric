using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPickUpBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player == null)
            return;

        Magnet playerMagnet = player.GetComponent<Magnet>();
        if (playerMagnet != null)
        {
            playerMagnet.IsActive = true;
            GetComponent<AudioSpawn>()?.Spawn();
        }

        Destroy(gameObject);
    }
}
