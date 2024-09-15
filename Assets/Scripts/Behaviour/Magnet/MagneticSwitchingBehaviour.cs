using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticSwitchingBehaviour : MonoBehaviour
{
    private Magnet _magnet;
    private void Awake()
    {
        _magnet = gameObject.GetComponent<Magnet>();
    }

    public bool Switch()
    {
        if (_magnet == null)
            return false;

        Magnet.Pole pole = _magnet.Polarization;
        if (pole == Magnet.Pole.south)
            _magnet.Polarization = Magnet.Pole.north;

        if (pole == Magnet.Pole.north)
            _magnet.Polarization = Magnet.Pole.south;

        _magnet.ChangeMagneticPole();
        return true;
    }
}
