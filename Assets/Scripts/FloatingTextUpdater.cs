using TMPro;
using UnityEngine;

public class FloatingTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshPro targetText;

    private void Reset()
    {
        if (targetText == null)
            targetText = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        // Simple runtime sanity check; replace with your actual text.
        SetText("Test");
    }

    public void SetText(string message)
    {
        if (targetText == null)
        {
            Debug.LogWarning($"{nameof(FloatingTextUpdater)} on {name} is missing a TextMeshPro reference.");
            return;
        }

        targetText.text = message;
    }
}


