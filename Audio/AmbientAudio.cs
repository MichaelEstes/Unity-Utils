using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [SerializeField]
    AmbientAudioEvent ambientAudioEvent;

    [SerializeField]
    AudioSource audioSource;

    void Start()
    {
        ambientAudioEvent.PlayLoop(audioSource);
    }
}
