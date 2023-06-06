using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Events/AmbientAudioEvent")]

public class AmbientAudioEvent : AudioEvent
{

    public void PlayLoop(AudioSource source)
    {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume.maxValue;
        source.pitch = pitch.maxValue;
        source.loop = true;
        source.Play();
    }
}
