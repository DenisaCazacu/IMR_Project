using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingTextUpdater : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        [TextArea(2, 5)]
        public string text;
        public AudioClip audioClip;
        [Tooltip("Audio reply that plays after the main audio finishes")]
        public AudioClip replyAudioClip;
    }

    [SerializeField] private TextMeshPro targetText;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Dialogue Settings")]
    [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();
    
    [Tooltip("Additional time to add after audio finishes playing")]
    [SerializeField] private float additionalWaitAfterAudio = 0.5f;
    
    [Tooltip("Minimum wait time if no audio is provided")]
    [SerializeField] private float minimumWaitTimeNoAudio = 2f;

    private Coroutine _currentDialogueCoroutine;

    private void Reset()
    {
        if (targetText == null)
            targetText = GetComponentInChildren<TextMeshPro>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    /// <summary>
    /// Starts playing through all dialogues in sequence
    /// </summary>
    ///
    ///
    ///
    [ContextMenu("Start Dialogue")]
    public void StartDialogue()
    {
        if (_currentDialogueCoroutine != null)
        {
            StopCoroutine(_currentDialogueCoroutine);
        }
        
        _currentDialogueCoroutine = StartCoroutine(PlayDialogueSequence());
    }

    /// <summary>
    /// Stops the current dialogue playback
    /// </summary>
    public void StopDialogue()
    {
        if (_currentDialogueCoroutine != null)
        {
            StopCoroutine(_currentDialogueCoroutine);
            _currentDialogueCoroutine = null;
        }
        
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator PlayDialogueSequence()
    {
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning($"{nameof(FloatingTextUpdater)} on {name} has no dialogues to play.");
            yield break;
        }

        if (targetText == null)
        {
            Debug.LogWarning($"{nameof(FloatingTextUpdater)} on {name} is missing a TextMeshPro reference.");
            yield break;
        }

        foreach (var dialogue in dialogues)
        {
            // Update text
            targetText.text = dialogue.text;
            
            // Play main audio if available
            if (dialogue.audioClip != null && audioSource != null)
            {
                audioSource.clip = dialogue.audioClip;
                audioSource.Play();
                float mainAudioDuration = dialogue.audioClip.length;
                yield return new WaitForSeconds(mainAudioDuration + additionalWaitAfterAudio);
            }
            else
            {
                // No audio provided, use minimum wait time
                yield return new WaitForSeconds(minimumWaitTimeNoAudio);
            }
            
            // Play reply audio if available
            if (dialogue.replyAudioClip != null && audioSource != null)
            {
                audioSource.clip = dialogue.replyAudioClip;
                audioSource.Play();
                float replyAudioDuration = dialogue.replyAudioClip.length;
                yield return new WaitForSeconds(replyAudioDuration + additionalWaitAfterAudio);
            }
        }
        
        _currentDialogueCoroutine = null;
    }

    /// <summary>
    /// Legacy method for setting text directly (for backwards compatibility)
    /// </summary>
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
