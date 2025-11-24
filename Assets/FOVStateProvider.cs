using UnityEngine;

public class FOVStateProvider : MonoBehaviour
{
    [SerializeField] private Camera camera;

    
    public static FOVStateProvider Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsObjectInFOV(Renderer target)
    {
        var cam = camera != null ? camera : Camera.main;
        if (cam == null || target == null) return false;
        var bounds = target.bounds;
        Vector3 center = bounds.center;
        Vector3 local = cam.transform.InverseTransformPoint(center);
        float halfFovRad = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float x = local.x;
        float z = local.z;
        if (z <= 0) return false;
        float maxX = Mathf.Tan(halfFovRad) * z;
        float r = bounds.extents.x;
        if (x + r < -maxX || x - r > maxX) return false;
        return true;
    }
}
