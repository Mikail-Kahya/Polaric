using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointSave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.gameObject.GetComponent<PlayerCharacter>();

        if (player == null)
            return;

        other.gameObject.GetComponent<Death>().Spawn = transform.position;
        Destroy(gameObject);
    }
}
