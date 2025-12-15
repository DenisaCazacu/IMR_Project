using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Rendering;

public class AffordanceRendererAssigner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        MaterialPropertyBlockHelper blockHelper = GetComponentInChildren<MaterialPropertyBlockHelper>();
        blockHelper.rendererTarget = rend;
    }
}
