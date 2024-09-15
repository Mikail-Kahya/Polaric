using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public enum Pole
    {
        north,
        south
    }

    [SerializeField] private Material _materialNorth = null;
    [SerializeField] private Material _materialSouth = null;
    [SerializeField] private Material _materialInactive = null;
    [SerializeField] private GameObject _magneticFieldTemplate = null;

    [SerializeField] private Pole _polarization;

    private MeshRenderer _meshRenderer = null;
    private MagneticField _magneticField = null;
    private Rigidbody _rigidBody = null;
    [SerializeField] private bool _isActive = false;
    [SerializeField] private bool _isStatic = false;
    private bool _isAttractedToStatics = false;

    [SerializeField] private float _attractRadius = 10.0f;
    [SerializeField] private float _attractPower = 10.0f;
    [SerializeField] private float _maxForce = 50.0f;

    private static string MAGNET_TAG = "Magnetic";

    #region GETTER / SETTER
    public static string Tag
    {
        get { return MAGNET_TAG; }
    }

    public float AttractRadius
    {
        get { return _attractRadius; }
        set { _attractRadius = value; }
    }

    public float AttractPower
    {
        get { return _attractPower; }
    }

    public Pole Polarization
    {
        get { return _polarization; }
        set { _polarization = value; }
    }

    public bool IsActive
    {
        get { return _isActive; }
        set { ToggleActive(value); }
    }

    // Used to determine whether or not the magnet is attracted (for attraction controls)
    public bool IsAttractedToStatic
    {
        get { return _isAttractedToStatics; }
    }

    public bool IsStatic
    {
        get { return _isStatic; }
    }

    public MagneticField MagneticField 
    { 
        get { return _magneticField; } 
    }

    #endregion GETTER / SETTER
    private void Awake()
    {
        // Gets added to MagnetManager singleton
        MagnetManager.Instance.RegisterMagnet(this);
        gameObject.tag = MAGNET_TAG;

        RenderField();

        // object isn't static == object is dynamic => rigidbody used for movement
        if (_isStatic) 
            return;
        
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void ChangeMagneticPole()
    {
        if (_meshRenderer == null || !_isActive)
            return;

        _magneticField.ChangeMagneticPole(_polarization);

        switch (_polarization)
        {
            case Pole.north:
                _meshRenderer.material = _materialNorth;
                break;

            case Pole.south:
                _meshRenderer.material = _materialSouth;
                break;
        }
    }

    private void CreateMagneticField()
    {        
        GameObject magneticFieldObj = Instantiate(_magneticFieldTemplate);

        // parent the magnetic field to the magnet
        magneticFieldObj.transform.parent = gameObject.transform;

        magneticFieldObj.transform.position = gameObject.transform.position;
        magneticFieldObj.transform.localScale = new Vector3(_attractRadius * 2, _attractRadius * 2, 0.1f);

        _magneticField = magneticFieldObj.GetComponent<MagneticField>();
    }

    public void UpdateRigidBody(List<Magnet> closeMagnets)
    {
        if(!_isActive || _rigidBody == null) 
            return;

        SetMagnetPhysics(closeMagnets.Count > 0);
        if (closeMagnets.Count < 0)
            return;
            
        // Calculate the net force of all magnets closeby
        // This netforce is to calculate the netforce applied on itself by the surrounding magnets
        Vector2 netForce = Vector2.zero;

        foreach (Magnet magnet in closeMagnets)
        {
            Vector2 direction = magnet.transform.position - gameObject.transform.position;
            float distanceSqr = direction.sqrMagnitude;

            // Less affected the further the magnet is
            float intensity = Mathf.Pow(magnet.AttractRadius, 2) / distanceSqr;
            Vector2 force = direction * magnet.AttractPower * intensity;

            // repel if same poles
            if (magnet.Polarization == _polarization)
                netForce -= force;
            else
                netForce += force;
        }

        if (netForce.sqrMagnitude > _maxForce * _maxForce)
            netForce = netForce.normalized * _maxForce;

        _rigidBody.AddForce(netForce);
    }

    private void SetMagnetPhysics(bool useMagnetPhysics)
    {
        // The magneticfield ignores gravity for dynamic movement
        _rigidBody.useGravity = !useMagnetPhysics;
        _isAttractedToStatics = useMagnetPhysics; // Determines if you should have fly controls enabled (up, down, left and right)
    }

    private void ToggleActive(bool isActive)
    {
        // Used to deactivate / activate magnets and their attraction
        _isActive = isActive;
        _magneticField.gameObject.SetActive(isActive);

        if (!isActive && _meshRenderer != null)
            _meshRenderer.material = _materialInactive;
        else
            ChangeMagneticPole();
    }

    public void UpdateMagneticField(List<Magnet> closeMagnets)
    {
        // Sets the mangeticfield of magnets this one is in contact with to active
        // Enable other magnets, not own
        foreach (Magnet magnet in closeMagnets)
        {
            // Statics won't ever be affected by other magnets and thus don't update the others
            if (_isStatic)
                continue;

            if (magnet._magneticField != null)
                magnet._magneticField.IsActive = true;
        }
    }

    // Render in the editor for visual confirmation (does not represent in game version, just for radius)
    [ContextMenu("Render")]
    void RenderField()
    {
        // Delete the old field used from the editor to setup the new one
        int nrChildren = transform.childCount;
        if (nrChildren != 0)
        {
            // Find the field child to destroy (there should only be one)
            GameObject oldField = null;

            for (int index = 0; index < nrChildren; index++)
            {
                GameObject child = transform.GetChild(index).gameObject;
                if (child.GetComponent<MagneticField>())
                {
                    oldField = child;
                    break;
                }
            }
               
            if (Application.isPlaying)
                Destroy(oldField);
            else
                DestroyImmediate(oldField);
        }
        

        _meshRenderer = gameObject.GetComponent<MeshRenderer>();

        // In case the visuals are a child and not part of this gameobject (e.g. player)
        if (_meshRenderer == null)
            _meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();

        CreateMagneticField();
        ToggleActive(_isActive);
        ChangeMagneticPole();
    }
}