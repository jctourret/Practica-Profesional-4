using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXAudioSource : MonoBehaviour
{
    AudioSource audioSource;
    AudioSettings audioSettings;

    void Awake()
    {
        audioSettings = AudioSettings.audioSettings;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioSettings.GetSFXVolume();
        audioSettings.AddMeToSFXAudioSources(audioSource);
    }

    void OnDestroy()
    {
        audioSettings.RemoveMeFromSFXAudioSources(audioSource);
    }
}
