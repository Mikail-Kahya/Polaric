using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    #region Singleton
    private static MagnetManager _instance = null;

    public static MagnetManager Instance
    {
        get
        {
            if (_instance == null && !ApplicationQuitting)
            {
                _instance = FindObjectOfType<MagnetManager>();
                if (_instance == null)
                {
                    GameObject newInstance = new GameObject("Singleton_MagnetManager");
                    _instance = newInstance.AddComponent<MagnetManager>();
                }
            }

            return _instance;
        }
    }

    public static bool Exists
    {
        get { return _instance != null; }
    }

    public static bool ApplicationQuitting = false;

    protected virtual void OnApplicationQuit()
    {
        ApplicationQuitting = true;
    }

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    private List<Magnet> _magnetList = new List<Magnet>();

    public void RegisterMagnet(Magnet magnet)
    {
        if (!_magnetList.Contains(magnet))
            _magnetList.Add(magnet);
    }

    public void UnRegisterMagnet(Magnet spawnPoint)
    {
        // checks if it contains and deletes the contained element
        // makes list shorter at O(1)
        _magnetList.Remove(spawnPoint);
    }

    private void FixedUpdate()
    {
        // reset magneticfields
        foreach (Magnet magnet in _magnetList)
            magnet.MagneticField.IsActive = false;


        // Get all closest magnets
        // (current magnet has to be in range of other magnet's attraction radius)
        foreach (Magnet magnet in _magnetList)
        {
            if (!magnet.IsActive)
                return;

            List<Magnet> closeMagnets = new List<Magnet>();
            foreach (Magnet otherMagnet in _magnetList)
            {
                // skip same object
                if (otherMagnet == magnet || !otherMagnet.IsActive)
                    continue;

                Vector2 direction = magnet.transform.position - otherMagnet.transform.position;
                float distanceSqr = direction.sqrMagnitude;

                if (distanceSqr < otherMagnet.AttractRadius * otherMagnet.AttractRadius)
                {
                    closeMagnets.Add(otherMagnet);
                    if (magnet.IsStatic)
                        break;
                }
            }

            magnet.UpdateRigidBody(closeMagnets);
            magnet.UpdateMagneticField(closeMagnets);
        }
    }

    private void Update()
    {
        // remove all objects that are destroyed
        _magnetList.RemoveAll(s => s == null);

        // predicate = functor
        // uses callback arrow notation
        // while (_spawnPoints.Remove(null)) { }
    }
}
