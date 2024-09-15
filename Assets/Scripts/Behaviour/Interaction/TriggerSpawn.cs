using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawn : MonoBehaviour
{
    [SerializeField] GameObject _spawnTemplate;
    private GameObject _spawnedObj = null;

    private void OnTriggerEnter()
    {
        if (_spawnTemplate == null || _spawnedObj != null)
            return;

        _spawnedObj = Instantiate(_spawnTemplate);
        _spawnedObj.transform.position = gameObject.transform.position;
        _spawnedObj.transform.rotation = gameObject.transform.rotation;
    }
}
