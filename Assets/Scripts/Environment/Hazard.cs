using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private GameObject _hazardVisualTemplate;
    [SerializeField] private GameObject _EndHazardPoint;
    [SerializeField] private float _hazardSize = 1.0f;
    [SerializeField] private bool _rendered = false;
    private const string HAZARD_TAG = "Hazard";
    
    private void Awake()
    {
        // In case I forget to manually render the visuals in the editor
        if (!_rendered)
            CreateHazard();
    }

    void CreateHazard()
    {
        // Fill the given area in depth and width with a given prefab (area determined by 2 points)

        // In case a prefab or in game object doesn't exist
        if (_hazardVisualTemplate == null)
        {
            Debug.Log("No hazard template: " + gameObject.name);
            return;
        }

        if (_EndHazardPoint == null)
        {
            Debug.Log("No end point for hazards");
            return;
        }

        BoxCollider visualBoxCollider = _hazardVisualTemplate.GetComponent<BoxCollider>();
        visualBoxCollider.enabled = false;
        if (visualBoxCollider == null)
        {
            Debug.Log("Hazard template does not have a box collider");
            return;
        }

        // Setup death collider triggerbox
        // The scale affects how many you place down and how big the hitbox gets
        Vector3 scaling = _hazardVisualTemplate.transform.localScale;
        float width = Mathf.Abs(visualBoxCollider.size.x * scaling.x * _hazardSize);
        float height = Mathf.Abs(visualBoxCollider.size.y * scaling.y * _hazardSize);
        float depth = Mathf.Abs(visualBoxCollider.size.z * scaling.z * _hazardSize);

        Vector3 startToEnd = _EndHazardPoint.transform.localPosition;

        // Better too much, than not enough
        int rows = Mathf.CeilToInt(startToEnd.x / width);
        int cols = Mathf.CeilToInt(startToEnd.z / depth);

        float totalWidth = width * rows;
        float totalDepth = depth * cols;

        BoxCollider hazardBoxCollider = gameObject.GetComponent<BoxCollider>();
        if (hazardBoxCollider == null)
            hazardBoxCollider = gameObject.AddComponent<BoxCollider>();

        // A warning seems to occure regarding to the hazard collision box being negative but this never occurs
        hazardBoxCollider.size = new Vector3(totalWidth, height, totalDepth);
        hazardBoxCollider.center = new Vector3(totalWidth * 0.5f, height * 0.5f, totalDepth * 0.5f);

        // Add visuals that don't effect the gameplay
        AddHazardVisuals(width, rows, cols);
    }


    void AddHazardVisuals(float hazardSize, int nrRows, int nrCols)
    {
        // Fill up hazards in an orthographic grid (not in Y-axis)
        Vector3 hazardPos = Vector3.zero;
        hazardPos.z -= hazardSize * nrRows * 0.5f;

        for (int row = 0; row < nrRows; ++row)
        {
            hazardPos.z = 0;
            for (int col = 0; col < nrCols; ++col)
            {
                AddHazard(hazardPos);
                hazardPos.z += hazardSize;
            }
            hazardPos.x += hazardSize;
        }
    }
    void AddHazard(Vector3 position)
    {
        // Add a hazardObject without the collision box active, just the visuals
        GameObject visual = Instantiate(_hazardVisualTemplate);
        visual.transform.parent = gameObject.transform;
        visual.transform.localPosition = position; 
        visual.transform.rotation = gameObject.transform.rotation;
        visual.transform.localScale = _hazardVisualTemplate.transform.localScale * _hazardSize;
    }

    // Render in the editor for visual confirmation
    [ContextMenu("Render")]
    void RenderHazard()
    {
        ClearHazard();
        CreateHazard();
        _rendered = true;
    }

    void ClearHazard()
    {
        // Destroy all prior objects automatically rather than manually
        // Store in array because deletion of a child results in automatic resizing
        // => GetChild(index) would not work during destruction loop
        int nrChildren = transform.childCount;
        GameObject[] childrenArr = new GameObject[nrChildren];
        for (int index = 0; index < nrChildren; index++)
            childrenArr[index] = transform.GetChild(index).gameObject;


        for (int index = 0; index < nrChildren; index++)
        {
            GameObject child = childrenArr[index];
            if (child.tag == HAZARD_TAG)
                continue;

            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Use one big hitbox rather
        other.gameObject.GetComponent<Death>()?.Respawn(); // Teleports and resets velocity
    }
}
