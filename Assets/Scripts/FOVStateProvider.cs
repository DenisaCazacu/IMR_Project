using UnityEngine;

public class FOVStateProvider : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

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
        Camera cameraToUse = targetCamera != null ? targetCamera : Camera.main;
        if (cameraToUse == null || target == null) return false;
        var bounds = target.bounds;
        Vector3 center = bounds.center;
        Vector3 local = cameraToUse.transform.InverseTransformPoint(center);
        if (local.z <= 0f) return false;
        float halfV = cameraToUse.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float halfH = Mathf.Atan(Mathf.Tan(halfV) * cameraToUse.aspect);
        float maxX = Mathf.Tan(halfH) * local.z;
        float r = bounds.extents.magnitude;
        if (local.x + r < -maxX || local.x - r > maxX) return false;
        return true;
    }
}
