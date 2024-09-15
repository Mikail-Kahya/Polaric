using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Magnet;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private Material _materialNorth = null;
    [SerializeField] private Material _materialSouth = null;

    private bool _isActive = false;

    private const float _defaultAlpha = 0.05f;
    private const float _activeAlpha = 0.4f;

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; } 
    }

    private MeshRenderer _meshRenderer = null;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
            
        // Make new material instances to reduce the alpha when not active
        _materialNorth = new Material(_materialNorth); 
        _materialSouth = new Material(_materialSouth);
    }

    private void OnDestroy()
    {
        // Destroy self made instances
        Destroy(_materialNorth);
        Destroy(_materialSouth);
    }

    private void Update()
    {
        float alpha = (_isActive) ? _activeAlpha : _defaultAlpha;

        ChangeMaterialAlpha(_materialNorth, alpha);
        ChangeMaterialAlpha(_materialSouth, alpha);
    }

    public void ChangeMagneticPole(Pole polarization)
    {
        if (_meshRenderer == null)
            return;

        switch (polarization)
        {
            case Pole.north:
                _meshRenderer.material = _materialNorth;
                break;

            case Pole.south:
                _meshRenderer.material = _materialSouth;
                break;
        }
    }

    void ChangeMaterialAlpha(Material material, float alpha)
    {
        Color materialColor = material.color;
        materialColor.a = alpha;
        material.color = materialColor;
    }
}
