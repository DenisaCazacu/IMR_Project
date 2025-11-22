using System;
using UnityEngine;

public class MovementHack : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Vector3 initialCameraLocalPosition;

    private void Start()
    {
        if (cameraTransform != null)
            initialCameraLocalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (cameraTransform == null) return;
        // Calculate offset from initial local position
        Vector3 offset = cameraTransform.localPosition - initialCameraLocalPosition;
        // Move parent by offset in world space
        transform.position += transform.TransformVector(offset);
        // Reset camera's local position to initial offset
        cameraTransform.localPosition = initialCameraLocalPosition;
    }
}
