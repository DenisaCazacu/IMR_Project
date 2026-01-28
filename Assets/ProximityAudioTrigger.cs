using UnityEngine;

public class ProximityAudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private bool isActive = false;

    public void EnableTrigger()
    {
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player passed near the object => playing audio");
            audioSource.Play();
            isActive = false; // Only trigger once
        }
    }
}
